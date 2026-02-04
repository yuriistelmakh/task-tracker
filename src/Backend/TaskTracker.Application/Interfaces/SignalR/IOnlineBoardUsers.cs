using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskTracker.Application.Interfaces.SignalR;

public interface IOnlineBoardUsers
{
    Task UserJoinedAsync(int boardId, int userId, string connectionId);
    Task UserLeftAsync(int boardId, int userId, string connectionId);
    Task<IReadOnlyCollection<int>> GetUsersAsync(int boardId);
    Task<(int BoardId, int UserId)?> TryGetByConnectionIdAsync(string connectionId);
}
