namespace TaskTracker.Domain.DTOs.Auth;

public class AuthTokens
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }
}
