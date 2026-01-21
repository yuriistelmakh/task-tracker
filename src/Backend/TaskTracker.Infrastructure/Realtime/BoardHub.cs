using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace TaskTracker.Infrastructure.Realtime;

public class BoardHub : Hub<IBoardClient>
{
    public async Task JoinBoard(int boardId)
    {
        var groupName = $"Board_{boardId}";
        Console.WriteLine($"New connection to {boardId} has been made");
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveBoard(int boardId)
    {
        var groupName = $"Board_{boardId}";
        Console.WriteLine($"Someone left {boardId}");
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
