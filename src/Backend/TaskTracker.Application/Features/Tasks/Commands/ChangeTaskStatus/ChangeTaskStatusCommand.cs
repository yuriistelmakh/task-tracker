using MediatR;

namespace TaskTracker.Application.Features.Tasks.Commands.ChangeStatus;

public class ChangeTaskStatusCommand : IRequest<Result>
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public bool IsComplete { get; set; }

    public int UpdatedBy { get; set; }
}
