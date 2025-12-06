namespace TaskTracker.Domain.DTOs.Auth;

public class SignupRequest
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }
}
