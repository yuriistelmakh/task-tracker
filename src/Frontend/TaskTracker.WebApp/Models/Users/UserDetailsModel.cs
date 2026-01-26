namespace TaskTracker.WebApp.Models.Users;

public class UserDetailsModel
{
    public int Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Tag { get; set; }

    public required string Email { get; set; }

    public string? AvatarUrl { get; set; }
}
