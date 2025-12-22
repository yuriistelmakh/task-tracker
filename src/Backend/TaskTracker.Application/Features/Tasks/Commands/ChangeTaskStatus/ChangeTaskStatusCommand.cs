using MediatR;

namespace TaskTracker.Application.Features.Tasks.Commands.ChangeStatus;

public class ChangeTaskStatusCommand : IRequest<bool>
{
    public int Id { get; set; }

    public bool IsComplete { get; set; }

    public int UpdatedBy { get; set; }
}
