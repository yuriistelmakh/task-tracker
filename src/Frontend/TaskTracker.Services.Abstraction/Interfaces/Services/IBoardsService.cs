using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardsService
{
    Task<Result<IEnumerable<BoardSummaryDto>>> GetAllAsync();

    Task<Result<BoardDetailsDto>> GetAsync(int id);
}
