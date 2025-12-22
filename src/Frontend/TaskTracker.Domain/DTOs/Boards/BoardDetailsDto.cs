using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Domain.DTOs.Boards;

public class BoardDetailsDto
{
     public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public bool IsArchived { get; set; }

    public required UserSummaryDto Owner { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ColumnSummaryDto> Columns { get; set; } = [];
}
