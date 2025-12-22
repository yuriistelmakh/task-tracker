using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public sealed class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly ISessionCacheService _sessionCache;

    private static readonly AuthenticationState AnonymousState =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private AuthenticationState? _cachedState;

    public CustomAuthStateProvider(
        ProtectedLocalStorage localStorage,
        ISessionCacheService sessionCacheService)
    {
        _localStorage = localStorage;
        _sessionCache = sessionCacheService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_cachedState is not null)
        {
            return _cachedState;
        }

        try
        {
            var result = await _localStorage.GetAsync<string>("s_id");

            if (!result.Success || string.IsNullOrWhiteSpace(result.Value))
            {
                return AnonymousState;
            }

            var sid = result.Value;

            var userData = _sessionCache.GetSessionData(sid);

            if (userData is null || string.IsNullOrWhiteSpace(userData.AccessToken))
            {
                await _localStorage.DeleteAsync("s_id");
                return AnonymousState;
            }

            try
            {
                var claims = JwtParser.ParseClaimsFromJwt(userData.AccessToken);
                var identity = new ClaimsIdentity(claims, "CustomAuth");
                var user = new ClaimsPrincipal(identity);

                _sessionCache.CurrentSessionId = sid;

                return _cachedState = new AuthenticationState(user);
            }
            catch
            {
                return AnonymousState;
            }
        }
        catch 
        {
            return AnonymousState;
        }
    }

    public void NotifyLoggedIn(string sid)
    {
        var userData = _sessionCache.GetSessionData(sid);
        if (userData is not null)
        {
            var claims = JwtParser.ParseClaimsFromJwt(userData.AccessToken);
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            _cachedState = new AuthenticationState(new ClaimsPrincipal(identity));

            _sessionCache.CurrentSessionId = sid;

            NotifyAuthenticationStateChanged(Task.FromResult(_cachedState));
        }
    }

    public async Task NotifyLoggedOut()
    {
        await _localStorage.DeleteAsync("s_id");
        NotifyAuthenticationStateChanged(Task.FromResult(AnonymousState));
    }
}