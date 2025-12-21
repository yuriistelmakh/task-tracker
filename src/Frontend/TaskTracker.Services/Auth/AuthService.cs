using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;
    private readonly ISessionCacheService _sessionCacheService;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(
        IAuthApi authApi,
        ISessionCacheService sessionCacheService,
        ProtectedLocalStorage protectedLocalStorage,
        AuthenticationStateProvider authStateProvider)
    {
        _authApi = authApi;
        _sessionCacheService = sessionCacheService;
        _protectedLocalStorage = protectedLocalStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<AuthErrorType> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _authApi.LoginAsync(request);
            return await HandleAuthResponse(response.Content);
        }
        catch (Exception)
        {
            return AuthErrorType.None;
        }
    }

    public async Task<AuthErrorType> SignupAsync(SignupRequest request)
    {
        try
        {
            var response = await _authApi.SignupAsync(request);
            return await HandleAuthResponse(response.Content);
        }
        catch (Exception)
        {
            return AuthErrorType.None;
        }
    }

    public async Task LogoutAsync()
    {
        var sessionId = _sessionCacheService.CurrentSessionId;

        if (!string.IsNullOrWhiteSpace(sessionId))
        {
            _sessionCacheService.RemoveSession(sessionId);
        }

        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            await customProvider.NotifyLoggedOut();
        }
    }

    private async Task<AuthErrorType> HandleAuthResponse(AuthResponse? content)
    {
        if (content is null)
        {
            return AuthErrorType.Unknown;
        }

        if (content.ErrorType != AuthErrorType.None)
        {
            return content.ErrorType;
        }

        if (content.UserData is null || string.IsNullOrWhiteSpace(content.UserData.AccessToken))
        {
            return AuthErrorType.Unknown;
        }

        var sessionId = _sessionCacheService.CreateSession(content.UserData);
        await _protectedLocalStorage.SetAsync("s_id", sessionId);
        _sessionCacheService.CurrentSessionId = sessionId;

        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            customProvider.NotifyLoggedIn(sessionId);
        }

        return AuthErrorType.None;
    }
}