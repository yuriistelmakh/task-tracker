using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Realtime;

public interface IBoardClient
{
    Task ReceiveUserJoinedBoard(int boardId, int userId);
    Task ReceiveTaskCreated(int boardId, TaskSummaryDto taskDto);
    Task ReceiveTestSignal(int boardId);
    Task ReceiveBoardChanged(int boardId);
    Task ReceiveTaskUpdated(int boardId, TaskSummaryDto taskDto);
    Task ReceiveTaskDeleted(int boardId, int taskId);
    Task ReceiveColumnCreated(int boardId, ColumnSummaryDto columnDto);
    Task ReceiveColumnUpdated(int boardId, ColumnSummaryDto columnDto);
    Task ReceiveColumnDeleted(int boardId, int columnId);
    Task ReceiveUserJoined(int boardId, int userId);
    Task ReceiveUserLeft(int boardId, int userId);
    Task ReceiveOnlineUsersUpdated(int boardId, IReadOnlyCollection<int> users);
}
