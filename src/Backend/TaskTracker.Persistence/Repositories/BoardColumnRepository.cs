using System.Data;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class BoardColumnRepository : Repository<BoardColumn, int>, IBoardColumnRepository
{
    public BoardColumnRepository(IDbTransaction transaction) : base(transaction)
    {
    }
}
