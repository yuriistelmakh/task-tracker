namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface ICurrentUserService
{
    Task<string?> GetUserDisplayName();
}
