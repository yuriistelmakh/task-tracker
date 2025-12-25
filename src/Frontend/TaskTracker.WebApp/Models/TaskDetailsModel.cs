using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Enums;

namespace TaskTracker.WebApp.Models;

public class TaskDetailsModel
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsComplete { get; set; }

    public required string ColumnTitle { get; set; }

    public UserSummaryModel? AssigneeModel { get; set; }
}
