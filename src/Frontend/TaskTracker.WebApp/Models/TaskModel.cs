using TaskTracker.Domain.Enums;

namespace TaskTracker.WebApp.Models;

public class TaskModel
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public int Order { get; set; }
}
