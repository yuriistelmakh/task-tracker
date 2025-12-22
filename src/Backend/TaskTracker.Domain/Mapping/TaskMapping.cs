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
            IsComplete = task.IsComplete
        };
}
