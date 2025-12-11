using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IBoardRepository : IRepository<Board, int>
{
    Task<IEnumerable<Board>> GetAllWithDetailsAsync(int userId);
    Task<Board?> GetDetailsAsync(int id);
    Task<int> AddMemberAsync(BoardMember boardMember);
    Task<IEnumerable<BoardMember>> GetMembersAsync(int boardId);
}
