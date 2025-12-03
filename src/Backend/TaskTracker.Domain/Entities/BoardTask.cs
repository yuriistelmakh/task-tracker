using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Domain.Entities;

[Table("Tasks")] 
public class BoardTask
{
    [Key]
    public int Id { get; set; }

    public int ColumnId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public int Priority { get; set; } = 1;

    public DateTime? DueDate { get; set; }

    public int Order { get; set; } = 0;

    public int? AssigneeId { get; set; }

    public bool IsComplete { get; set; } = false;

    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
}
