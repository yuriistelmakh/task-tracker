using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IBoardTaskRepository : IRepository<BoardTask, int>
{
    Task<bool> UpdateStatusAsync(int id, bool status, int updatedBy);

    Task<bool> UpdateOrderAsync(int id, int order);

    Task<bool> MoveToColumn(int id, int columnId);
}
