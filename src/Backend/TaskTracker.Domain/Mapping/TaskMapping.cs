using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class TaskMapping
{
    public static TaskSummaryDto ToTaskSummaryDto(this BoardTask task) =>
        new() 
        { 
            Id = task.Id,
            Title = task.Title,
            Order = task.Order,
            Priority = task.Priority,
            ColumndId = task.ColumnId,
            AssigneeId = task.AssigneeId,
            IsComplete = task.IsComplete,
            CreatedBy = task.CreatedBy
        };

    public static TaskDetailsDto ToTaskDetailsDto(this BoardTask task) =>
        new()
        {
            Id = task.Id,
            Title = task.Title,
            CreatedAt = task.CreatedAt,
            CreatedBy = task.CreatedBy,
            Description = task.Description,
            DueDate = task.DueDate,
            Priority = task.Priority,
            UpdatedAt = task.UpdatedAt,
            UpdatedBy = task.UpdatedBy,
            IsComplete = task.IsComplete
        };
}
