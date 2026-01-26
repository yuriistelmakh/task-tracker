using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Users;

public class MemberSummaryDto
{
    public int Id { get; set; }

    public required string DisplayName { get; set; }
    
    public required string Tag { get; set; }

    public BoardRole Role { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime JoinedAt { get; set; }
}
