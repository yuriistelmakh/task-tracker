using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public class SessionCacheService : ISessionCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _sessionTimeout = TimeSpan.FromHours(2);

    public string? CurrentSessionId { get; set; }

    public SessionCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public string CreateSession(AuthUserData userData)
    {
        var sessionId = Guid.NewGuid().ToString();
        _memoryCache.Set(sessionId, userData, _sessionTimeout);

        return sessionId;
    }

    public AuthUserData? GetSessionData(string sessionId)
    {
        if (_memoryCache.TryGetValue(sessionId, out AuthUserData? data))
        {
            return data;
        }

        return null;
    }

    public void UpdateSessionTokens(string sessionId, string newAccess, string newRefresh)
    {
        if (_memoryCache.TryGetValue(sessionId, out AuthUserData userData))
        {
            userData.AccessToken = newAccess;
            userData.RefreshToken = newRefresh;
        }
    }

    public void RemoveSession(string sessionId)
    {
        _memoryCache.Remove(sessionId);
    }
}