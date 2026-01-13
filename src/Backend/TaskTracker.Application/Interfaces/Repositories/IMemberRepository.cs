using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IMemberRepository : IRepository<BoardMember, int>
{
    Task<IEnumerable<BoardMember>> GetAllAsync(int boardId, int? page = null, int? pageSize = null);
    Task<BoardMember?> GetByIdsAsync(int boardId, int userId);
    Task<(IEnumerable<BoardMember> Items, int Count)> SearchByNameOrTag(int boardId, string? searchPrompt, int pageSize, int page = 1);
}
