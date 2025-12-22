using Refit;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IAuthApi
{
    [Post("/api/auth/signup")]
    Task<IApiResponse<AuthResponse?>> SignupAsync([Body] SignupRequest request);

    [Post("/api/auth/login")]
    Task<IApiResponse<AuthResponse?>> LoginAsync([Body] LoginRequest request);

    [Post("/api/auth/refresh")]
    Task<IApiResponse<AuthResponse?>> RefreshAsync([Body] RefreshRequest request);
}
