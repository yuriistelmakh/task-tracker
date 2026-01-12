using TaskTracker.WebApp.Models.Tasks;

namespace TaskTracker.WebApp.Models;

public class ColumnModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public List<TaskSummaryModel> Tasks { get; set; } = [];
    public int Order { get; set; }

    public bool IsAddTaskOpen { get; set; }
    public bool IsEditing { get; set; }
    public string? OldTitle { get; set; }
    public string? NewTaskTitle { get; set; }
}
