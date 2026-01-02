using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardsService
{
    Task<Result<int>> CreateAsync(CreateBoardRequest request);
    Task<Result<IEnumerable<BoardSummaryDto>>> GetAllAsync();

    Task<Result<BoardDetailsDto>> GetAsync(int id);

    Task<Result<IEnumerable<UserSummaryDto>>> GetMembersAsync(int id);

    Task<Result> ReorderColumnsAsync(int id, ReorderBoardColumnsRequest request);
}
