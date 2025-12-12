using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<AuthResult> RefreshTokenAsync(string accessToken, string refreshToken);
}
