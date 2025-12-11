using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequest request);

    Task<AuthResult> SignupAsync(SignupRequest request);
}
