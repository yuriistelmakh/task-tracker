namespace TaskTracker.Domain.DTOs.Auth;

public class AuthUserData
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public required string DisplayName { get; set; }

    public required string Tag { get; set; }
}
