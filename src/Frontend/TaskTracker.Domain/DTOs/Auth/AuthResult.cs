using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Auth;

public class AuthResult
{
    public bool IsSuccess { get; init; }
    public string? AccessToken { get; init; }
    public AuthErrorType ErrorType { get; init; }

    public static AuthResult Success(string accessToken) =>
        new() { IsSuccess = true, AccessToken = accessToken };

    public static AuthResult Failure(AuthErrorType error) =>
        new() { IsSuccess = false, ErrorType = error};
}