namespace TaskTracker.Domain.DTOs.Users;

public class UserSummaryDto
{
    public int Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Tag { get; set; }

    public string? AvatarUrl { get; set; }
}
