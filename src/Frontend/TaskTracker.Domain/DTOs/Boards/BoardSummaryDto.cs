using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Domain.DTOs.Boards;

public class BoardSummaryDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string DisplayColor { get; set; }

    public bool IsArchived { get; set; }

    public int TasksCount { get; set; }

    public int MembersCount { get; set; }

    public List<UserSummaryDto> Members { get; set; } = [];
}
