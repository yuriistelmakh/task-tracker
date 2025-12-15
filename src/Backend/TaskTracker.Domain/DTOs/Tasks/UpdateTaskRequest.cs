using System;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Tasks;

public class UpdateTaskRequest
{
    public int ColumnId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public DateTime? DueDate { get; set; }

    public int Order { get; set; }

    public int? AssigneeId { get; set; }

    public bool IsComplete { get; set; } = false;

    public int UpdatedBy { get; set; }
}
