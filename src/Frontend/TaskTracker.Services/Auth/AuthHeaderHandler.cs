using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly ISessionCacheService _sessionCache;

    public AuthHeaderHandler(ISessionCacheService sessionCache)
    {
        _sessionCache = sessionCache;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var sid = _sessionCache.CurrentSessionId;
        if (!string.IsNullOrWhiteSpace(sid))
        {
            var userData = _sessionCache.GetSessionData(sid);
            if (!string.IsNullOrWhiteSpace(userData?.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userData.AccessToken);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}