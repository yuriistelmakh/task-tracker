using System.Linq;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class ColumnMapping
{
    public static ColumnSummaryDto ToColumnSummaryDto(this BoardColumn column) =>
        new() 
        { 
            Id = column.Id,
            Title = column.Title,
            Order = column.Order,
            Tasks = column.Tasks.Select(t => t.ToTaskSummaryDto()).ToList()
        };
}
