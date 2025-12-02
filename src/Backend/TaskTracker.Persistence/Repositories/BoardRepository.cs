using TaskTracker.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class BoardRepository : Repository<Board, int>, IBoardRepository
{
    public BoardRepository(IConfiguration configuration) : base(configuration)
    {
    }
}
