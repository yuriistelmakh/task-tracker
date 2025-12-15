namespace TaskTracker.Domain.DTOs.Auth;

public class LoginRequest
{
    public string? Email { get; set; }

    public string? Tag { get; set; }

    public required string Password { get; set; }
}

