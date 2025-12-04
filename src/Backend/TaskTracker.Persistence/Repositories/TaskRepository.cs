using System.Data;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class TaskRepository : Repository<BoardTask, int>, IBoardTaskRepository
{
    public TaskRepository(IDbTransaction transaction) : base(transaction)
    {
    }
}
