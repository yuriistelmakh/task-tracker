namespace TaskTracker.Domain.DTOs.Users;

public class UpdateUserRequest
{
    public required string Tag { get; set; }

    public required string PasswordHash { get; set; }

    public required string DisplayName { get; set; }

    public required string AvatarUrl { get; set; }
}
