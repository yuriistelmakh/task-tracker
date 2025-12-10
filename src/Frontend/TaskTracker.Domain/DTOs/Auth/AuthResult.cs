using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Auth;

public class AuthResult
{
    public bool IsSuccess { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public AuthErrorType ErrorType { get; init; }

    public static AuthResult Success(string accessToken, string refreshToken) =>
        new() { IsSuccess = true, AccessToken = accessToken, RefreshToken = refreshToken };

    public static AuthResult Failure(AuthErrorType error) =>
        new() { IsSuccess = false, ErrorType = error};
}