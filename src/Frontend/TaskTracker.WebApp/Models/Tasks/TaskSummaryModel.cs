using TaskTracker.Domain.Enums;

namespace TaskTracker.WebApp.Models.Tasks;

public class TaskSummaryModel
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public int Order { get; set; }

    public bool IsComplete { get; set; } = false;

    public int ColumnId { get; set; }
}
