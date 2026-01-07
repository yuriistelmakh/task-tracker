using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IMemberRepository : IRepository<BoardMember, int>
{
    Task<IEnumerable<BoardMember>> GetAllAsync(int boardId);
    Task<BoardMember?> GetByIdsAsync(int boardId, int userId);
}
