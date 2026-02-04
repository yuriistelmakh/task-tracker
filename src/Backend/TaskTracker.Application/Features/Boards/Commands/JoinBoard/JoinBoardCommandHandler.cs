using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;

namespace TaskTracker.Application.Features.Boards.Commands.JoinBoard;

public class JoinBoardCommandHandler : IRequestHandler<JoinBoardCommand, Result>
{
    private readonly IBoardNotificator _boardNotificator;
    private readonly IOnlineBoardUsers _onlineBoardUsers;

    public JoinBoardCommandHandler(IBoardNotificator boardNotificator, IOnlineBoardUsers onlineBoardUsers)
    {
        _boardNotificator = boardNotificator;
        _onlineBoardUsers = onlineBoardUsers;
    }

    public async Task<Result> Handle(JoinBoardCommand request, CancellationToken cancellationToken)
    {
        await _onlineBoardUsers.UserJoinedAsync(request.BoardId, request.UserId, request.ConnectionId);

        var users = await _onlineBoardUsers.GetUsersAsync(request.BoardId);

        await _boardNotificator.OnlineUsersUpdatedAsync(request.BoardId, users);

        return Result.Success();
    }
}
