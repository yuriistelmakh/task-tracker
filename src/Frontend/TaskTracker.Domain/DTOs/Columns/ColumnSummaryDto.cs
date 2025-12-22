using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Domain.DTOs.Columns;

public class ColumnSummaryDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public int Order { get; set; }

    public List<TaskSummaryDto> Tasks { get; set; } = [];
}
