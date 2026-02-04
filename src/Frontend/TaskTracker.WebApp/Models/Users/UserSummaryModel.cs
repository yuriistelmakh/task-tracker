namespace TaskTracker.WebApp.Models.Users;

public class UserSummaryModel
{
    public int Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Tag { get; set; }

    public string? AvatarUrl { get; set; }
}
