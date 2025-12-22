using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Tasks;

public class TaskSummaryDto
{
    public int Id { get; set; }

    public int ColumndId { get; set; }

    public required string Title { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public int Order { get; set; } = 0;

    public int? AssigneeId { get; set; }
}
