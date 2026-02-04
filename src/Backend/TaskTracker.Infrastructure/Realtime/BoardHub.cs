using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Boards.Commands.JoinBoard;
using TaskTracker.Application.Features.Boards.Commands.LeaveBoard;
using TaskTracker.Application.Interfaces.SignalR;

namespace TaskTracker.Infrastructure.Realtime;

public class BoardHub : Hub<IBoardClient>
{
    private readonly IMediator _mediator;
    private readonly IOnlineBoardUsers _onlineBoardUsers;

    public BoardHub(IMediator mediator, IOnlineBoardUsers onlineBoardUsers)
    {
        _mediator = mediator;
        _onlineBoardUsers = onlineBoardUsers;
    }

    public async Task JoinBoard(int boardId, int userId)
    {
        var groupName = $"Board_{boardId}";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        var command = new JoinBoardCommand
        {
            BoardId = boardId,
            UserId = userId,
            ConnectionId = Context.ConnectionId
        };

        await _mediator.Send(command);
    }

    public async Task LeaveBoard(int boardId, int userId)
    {
        var groupName = $"Board_{boardId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        var command = new LeaveBoardCommand
        {
            BoardId = boardId,
            UserId = userId,
            ConnectionId = Context.ConnectionId
        };

        await _mediator.Send(command);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var info = await _onlineBoardUsers.TryGetByConnectionIdAsync(Context.ConnectionId);

        if (info is not null)
        {
            var groupName = $"Board_{info.Value.BoardId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await _mediator.Send(new LeaveBoardCommand
            {
                BoardId = info.Value.BoardId,
                UserId = info.Value.UserId,
                ConnectionId = Context.ConnectionId
            });
        }

        await base.OnDisconnectedAsync(exception);
    }
}
