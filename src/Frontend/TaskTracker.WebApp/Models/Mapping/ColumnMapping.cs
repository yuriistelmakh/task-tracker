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
            Tasks = dto.Tasks.Select(t => t.ToTaskModel()).OrderBy(t => t.Order).ToList()
        };
}
