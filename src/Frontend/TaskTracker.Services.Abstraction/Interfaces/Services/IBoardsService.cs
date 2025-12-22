using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardsService
{
    Task<IEnumerable<BoardSummaryDto>?> GetAllAsync();

    Task<BoardDetailsDto?> GetAsync(int id);
}
