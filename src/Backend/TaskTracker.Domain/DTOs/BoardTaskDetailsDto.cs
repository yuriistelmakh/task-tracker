using System;

namespace TaskTracker.Domain.DTOs;

public class BoardTaskDetailsDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public int Priority { get; set; } = 1;

    public DateTime? DueDate { get; set; }

    public int? AssigneeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}
