using MediatR;
using System;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<bool>
{
    public int Id { get; set; }

    public int ColumnId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public Priorities Priority { get; set; } = Priorities.Medium;

    public DateTime? DueDate { get; set; }

    public int Order { get; set; }

    public int? AssigneeId { get; set; }

    public bool IsComplete { get; set; } = false;

    public int UpdatedBy { get; set; }
}
