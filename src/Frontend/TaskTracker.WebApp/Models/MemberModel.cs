using TaskTracker.Domain.Enums;

namespace TaskTracker.WebApp.Models;

public class MemberModel
{
    public int Id { get; set; }

    public required string DisplayName { get; set; }

    public required string Tag { get; set; }    

    public BoardRole Role { get; set; }

    public string? AvatarUrl { get; set; }
}
