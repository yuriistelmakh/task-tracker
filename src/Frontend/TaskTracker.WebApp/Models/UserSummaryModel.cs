namespace TaskTracker.WebApp.Models;

public class UserSummaryModel
{
    public int Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Tag { get; set; }

    public string? AvatarUrl { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is UserSummaryModel other)
        {
            return Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
