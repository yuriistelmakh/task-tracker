using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;

namespace TaskTracker.Infrastructure.Realtime;

// Might switch to Redis
public class OnlineBoardUsers : IOnlineBoardUsers
{
    private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, int>> _boards = new();
    private readonly ConcurrentDictionary<string, (int BoardId, int UserId)> _connections = new();

    public Task<IReadOnlyCollection<int>> GetUsersAsync(int boardId)
    {
        if (_boards.TryGetValue(boardId, out var users))
        {
            return Task.FromResult<IReadOnlyCollection<int>>(users.Values.Distinct().ToList());
        }
        
        return Task.FromResult<IReadOnlyCollection<int>>(new List<int>());
    }

    public Task UserJoinedAsync(int boardId, int userId, string connectionId)
    {
        var users = _boards.GetOrAdd(boardId, _ => new());
        users[connectionId] = userId;
        _connections[connectionId] = (boardId, userId);
        return Task.CompletedTask;
    }

    public Task UserLeftAsync(int boardId, int userId, string connectionId)
    {
        if (_boards.TryGetValue(boardId, out var users))
        {
            users.TryRemove(connectionId, out _);
        }

        _connections.TryRemove(connectionId, out _);

        return Task.CompletedTask;
    }

    public Task<(int BoardId, int UserId)?> TryGetByConnectionIdAsync(string connectionId)
    {
        return Task.FromResult<(int BoardId, int UserId)?>(
            _connections.TryGetValue(connectionId, out var info) ? info : null);
    }
}
