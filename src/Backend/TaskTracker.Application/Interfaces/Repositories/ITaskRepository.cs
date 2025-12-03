using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IBoardTaskRepository : IRepository<BoardTask, int>
{
}
