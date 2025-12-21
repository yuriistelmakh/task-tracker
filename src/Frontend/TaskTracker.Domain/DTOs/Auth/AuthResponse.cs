using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Auth;

public class AuthResponse
{
    public AuthUserData? UserData { get; set; }

    public AuthErrorType ErrorType { get; set; }
}