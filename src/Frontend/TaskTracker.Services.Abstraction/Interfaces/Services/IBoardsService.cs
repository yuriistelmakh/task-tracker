using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardsService
{
    Task<Result<int>> CreateAsync(CreateBoardRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result<PagedResponse<BoardSummaryDto>>> GetAllAsync(int page, int pageSize);
    Task<Result<BoardDetailsDto>> GetAsync(int id);
    Task<Result> ReorderColumnsAsync(int id, ReorderBoardColumnsRequest request);
    Task<Result> UpdateAsync(int id, string? title = null, string? description = null, string? color = null);
}
