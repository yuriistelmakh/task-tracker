using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ISessionCacheService _sessionCache;
    private readonly NavigationManager _navigationManager;
    private readonly IAuthApi _authApi;
    private readonly IServiceProvider _serviceProvider;


    public AuthHeaderHandler(
        ISessionCacheService sessionCache,
        IAuthApi authApi,
        NavigationManager navigationManager,
        IServiceProvider serviceProvider)
    {
        _sessionCache = sessionCache;
        _authApi = authApi;
        _navigationManager = navigationManager;
        _serviceProvider = serviceProvider;
    }

    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sid = _sessionCache.CurrentSessionId;
        string? accessToken = null;

        if (!string.IsNullOrWhiteSpace(sid))
        {
            var userData = _sessionCache.GetSessionData(sid);
            if (!string.IsNullOrWhiteSpace(userData?.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userData.AccessToken);
            }
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
        {
            return response;
        }

        if (string.IsNullOrEmpty(sid))
        {
            return response;
        }

        var sessionData = _sessionCache.GetSessionData(sid);
        if (sessionData is null || string.IsNullOrWhiteSpace(sessionData.RefreshToken))
        {
            await ForceLogout();
            return response;
        }

        try
        {
            await _semaphore.WaitAsync(cancellationToken);

            var currentSessionData = _sessionCache.GetSessionData(sid);

            if (currentSessionData is not null && currentSessionData.RefreshToken != sessionData.RefreshToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", currentSessionData.AccessToken);
                return await base.SendAsync(request, cancellationToken);
            }

            var refreshRequest = new RefreshRequest
            {
                AccessToken = sessionData.AccessToken,
                RefreshToken = sessionData.RefreshToken
            };

            var refreshResponse = await _authApi.RefreshAsync(refreshRequest);

            if (refreshResponse.IsSuccessful && refreshResponse.Content is not null)
            {
                var newTokens = refreshResponse.Content;

                _sessionCache.UpdateSessionTokens(sid, newTokens.UserData.AccessToken, newTokens.UserData.RefreshToken);
                return await RetryRequestAsync(request, newTokens.UserData.AccessToken, cancellationToken);
            }
            else
            {
                _sessionCache.RemoveSession(sid);
                return response;
            }
        }
        catch (Exception)
        {
            await ForceLogout();
            return response;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<HttpResponseMessage> RetryRequestAsync(HttpRequestMessage request, string newToken, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task ForceLogout()
    {
        var currentUri = _navigationManager.Uri;
        if (!currentUri.Contains("/login"))
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                await authService.LogoutAsync();
            }

            _navigationManager.NavigateTo("/login");
        }
    }
}