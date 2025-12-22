using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.WebApp.Models.Mapping;

public static class TaskMapping
{
    public static TaskModel ToTaskModel(this TaskSummaryDto dto) =>
        new()
        { 
            Id = dto.Id,
            Title = dto.Title,
            IsComplete = dto.IsComplete,
            Order = dto.Order,
            Priority = dto.Priority,
        };
}
