using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Infrastructure.Realtime;

public class SignalRBoardNotificator : IBoardNotificator
{
    private readonly IHubContext<BoardHub, IBoardClient> _context;

    public SignalRBoardNotificator(IHubContext<BoardHub, IBoardClient> context)
    {
        _context = context;
    }

    public async Task TaskCreatedAsync(int boardId, TaskSummaryDto task)
    {
        var groupName = $"Board_{boardId}";

        await _context.Clients.Group(groupName).ReceiveTaskCreated(boardId, task);
    }

    public async Task TaskUpdatedAsync(int boardId, TaskSummaryDto task)
    {
        var groupName = $"Board_{boardId}";
        await _context.Clients.Group(groupName).ReceiveTaskUpdated(boardId, task);
    }

    public async Task TaskDeletedAsync(int boardId, int taskId)
    {
        var groupName = $"Board_{boardId}";
        await _context.Clients.Group(groupName).ReceiveTaskDeleted(boardId, taskId);
    }

    public async Task BoardChangedAsync(int boardId)
    {
        await _context.Clients.Group($"Board_{boardId}").ReceiveBoardChanged(boardId);
    }

    public async Task ColumnCreatedAsync(int boardId, ColumnSummaryDto column)
    {
        await _context.Clients.Group($"Board_{boardId}").ReceiveColumnCreated(boardId, column);
    }

    public async Task ColumnUpdatedAsync(int boardId, ColumnSummaryDto column)
    {
        await _context.Clients.Group($"Board_{boardId}").ReceiveColumnUpdated(boardId, column);
    }

    public async Task ColumnDeletedAsync(int boardId, int columnId)
    {
        await _context.Clients.Group($"Board_{boardId}").ReceiveColumnDeleted(boardId, columnId);
    }


    public async Task OnlineUsersUpdatedAsync(int boardId, IReadOnlyCollection<int> users)
    {
        await _context.Clients.Group($"Board_{boardId}").ReceiveOnlineUsersUpdated(boardId, users);
    }
}
