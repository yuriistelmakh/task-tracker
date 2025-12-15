using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Auth;

public class AuthResponse
{
    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public AuthErrorType? ErrorType { get; set; }
}
