using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IAuthService
{
    Task<AuthErrorType> LoginAsync(LoginRequest request);

    Task<AuthErrorType> SignupAsync(SignupRequest request);

    Task LogoutAsync();
}
