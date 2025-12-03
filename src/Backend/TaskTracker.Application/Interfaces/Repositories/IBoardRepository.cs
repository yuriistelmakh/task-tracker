using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IBoardRepository : IRepository<Board, int>
{
    Task<IEnumerable<Board>> GetAllWithOwnersAsync();
    Task<Board?> GetByIdDetailsAsync(int id);
}
