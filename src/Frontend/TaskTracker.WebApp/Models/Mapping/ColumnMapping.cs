using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.WebApp.Models.Mapping;

public static class ColumnMapping
{
    public static ColumnModel ToColumModel(this ColumnSummaryDto dto) =>
        new()
        {
            Id = dto.Id,
            Order = dto.Order,
            Title = dto.Title,
            NewTaskTitle = string.Empty,
            IsAddTaskOpen = false,
            CreatedBy = dto.CreatedBy,
            Tasks = dto.Tasks.Select(t => t.ToTaskSummaryModel()).OrderBy(t => t.Order).ToList()
        };
}
