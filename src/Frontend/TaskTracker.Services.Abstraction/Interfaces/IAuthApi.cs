using Refit;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Services.Abstraction.Interfaces;

public interface IAuthApi
{
    [Post("/api/auth/signup")]
    Task<AuthResponse?> SignupAsync([Body] SignupRequest request);

    [Post("/api/auth/login")]
    Task<string> LoginAsync([Body] LoginRequest request);

    [Post("/api/auth/refresh")]
    Task<AuthResponse?> RefreshAsync([Body] RefreshRequest request);
}
