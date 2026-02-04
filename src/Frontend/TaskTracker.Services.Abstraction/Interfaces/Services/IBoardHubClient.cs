using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardHubClient
{
    event Action<int>? OnBoardChanged;
    event Action<int, TaskSummaryDto>? OnTaskCreated;
    event Action<int, TaskSummaryDto>? OnTaskUpdated;
    event Action<int, int>? OnTaskDeleted;
    event Action<int, ColumnSummaryDto>? OnColumnUpdated;
    event Action<int, int>? OnColumnDeleted;
    event Action<int, ColumnSummaryDto>? OnColumnCreated;
    event Action<int, IReadOnlyCollection<int>>? OnOnlineUsersUpdated;

    Task ConnectAsync();
    Task DisconnectAsync();

    Task JoinBoardGroupAsync(int boardId, int userId);
    Task LeaveBoardGroupAsync(int boardId, int userId);
}
