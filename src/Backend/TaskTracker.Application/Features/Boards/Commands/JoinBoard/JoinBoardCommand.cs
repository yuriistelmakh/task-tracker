using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.JoinBoard;

public class JoinBoardCommand : IRequest<Result>
{
    public int BoardId { get; set; }
    
    public int UserId { get; set; }

    public required string ConnectionId { get; set; }
}
