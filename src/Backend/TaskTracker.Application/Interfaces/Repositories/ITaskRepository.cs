using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IBoardTaskRepository : IRepository<BoardTask, int>
{
    Task<bool> UpdateOrderAsync(int id, int order);
    Task<bool> MoveToColumn(int id, int columnId);
    Task<IEnumerable<BoardTask>> SearchAsync(int boardId, string? prompt, int pageSize);
    Task<IEnumerable<BoardTask>> GetAllByAssignee(int asigneeId);
    Task<BoardTask> GetByIdWithAttachments(int taskId);
}
