using System.Data;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class BoardTaskRepository : Repository<BoardTask, int>, IBoardTaskRepository
{
    public BoardTaskRepository(IDbTransaction transaction) : base(transaction)
    {
    }
}
