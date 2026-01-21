using MediatR;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<Result>
{
    public int Id { get; set; }

    public int BoardId { get; set; }
}
