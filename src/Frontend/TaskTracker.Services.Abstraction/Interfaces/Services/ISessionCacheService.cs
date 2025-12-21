using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface ISessionCacheService
{
    string CurrentSessionId { get; set; }

    AuthUserData? GetSessionData(string sessionId);

    string CreateSession(AuthUserData userData);

    void RemoveSession(string sessionId);
}
