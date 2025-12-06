using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
