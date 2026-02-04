using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.WebApp.Models.Tasks;

namespace TaskTracker.WebApp.Models.Mapping;

public static class TaskMapping
{
    public static TaskSummaryModel ToTaskSummaryModel(this TaskSummaryDto dto) =>
        new()
        { 
            Id = dto.Id,
            Title = dto.Title,
            IsComplete = dto.IsComplete,
            Order = dto.Order,
            Priority = dto.Priority,
            ColumnId = dto.ColumndId,
            CreatedBy = dto.CreatedBy
        };

    public static TaskDetailsModel ToTaskDetailsModel(this TaskDetailsDto dto) =>
        new()
        { 
            Id = dto.Id,
            Title = dto.Title,
            ColumnTitle = dto.ColumnTitle,
            CreatedAt = dto.CreatedAt,
            CreatedBy = dto.CreatedBy,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            UpdatedAt = dto.UpdatedAt,
            UpdatedBy = dto.UpdatedBy,
            IsComplete = dto.IsComplete,
            AssigneeModel = dto.AssigneeDto?.ToMemberModel() ?? null
        };
}
