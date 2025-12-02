using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;
using System.Data;

namespace TaskTracker.Persistence.Repositories;

public class BoardRepository : Repository<Board, int>, IBoardRepository
{
    public BoardRepository(IDbTransaction transaction) : base(transaction)
    {
    }
}
