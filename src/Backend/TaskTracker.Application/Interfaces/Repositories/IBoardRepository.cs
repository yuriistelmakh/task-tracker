using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IBoardRepository : IRepository<Board, int>
{
    Task<IEnumerable<Board>> GetAllWithDetailsAsync(int userId, int page, int pageSize);
    Task<int> GetCountAsync(int userId);
    Task<Board?> GetDetailsAsync(int id);
}
