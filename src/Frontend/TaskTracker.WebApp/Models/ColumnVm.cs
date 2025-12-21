namespace TaskTracker.WebApp.Models;

public class ColumnVm
{
    public required string Title { get; set; }

    public IEnumerable<TaskVm> Tasks { get; set; } = [];

    public int Order { get; set; }
}
