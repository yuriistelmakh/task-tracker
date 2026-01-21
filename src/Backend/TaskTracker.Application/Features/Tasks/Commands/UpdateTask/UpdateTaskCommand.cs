using MediatR;
using System;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<Result>
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public DateTime? DueDate { get; set; }

    public int? AssigneeId { get; set; }

    public bool IsComplete { get; set; } = false;

    public int UpdatedBy { get; set; }
}
