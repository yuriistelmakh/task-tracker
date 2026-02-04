using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.LeaveBoard;

public class LeaveBoardCommand : IRequest<Result>
{
    public int BoardId { get; set; }

    public int UserId { get; set; }

    public required string ConnectionId { get; set; }
}
