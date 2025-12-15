using System;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Tasks;

public class TaskDetailsDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public DateTime? DueDate { get; set; }

    public int? AssigneeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}
