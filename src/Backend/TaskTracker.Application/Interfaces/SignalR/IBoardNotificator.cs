using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Application.Interfaces.SignalR;

public interface IBoardNotificator
{
    Task BoardChangedAsync(int boardId);
    Task ColumnCreatedAsync(int boardId, ColumnSummaryDto column);
    Task ColumnDeletedAsync(int boardId, int columnId);
    Task ColumnUpdatedAsync(int boardId, ColumnSummaryDto column);
    Task TaskCreatedAsync(int boardId, TaskSummaryDto task);
    Task TaskDeletedAsync(int boardId, int taskId);
    Task TaskUpdatedAsync(int boardId, TaskSummaryDto task);
}
