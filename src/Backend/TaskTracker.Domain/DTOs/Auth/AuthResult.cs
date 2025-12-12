using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Auth;

public class AuthResult
{
    public string? AccessToken { get; set; }

    public RefreshToken? RefreshToken { get; set; }

    public AuthErrorType? ErrorType { get; set; }
}
