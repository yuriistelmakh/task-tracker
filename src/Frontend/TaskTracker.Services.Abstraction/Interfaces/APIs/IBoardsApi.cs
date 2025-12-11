using Refit;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardsApi
{
    [Get("/api/boards/my-boards/1")]
    Task<IApiResponse<IEnumerable<BoardSummaryDto?>>> GetAllAsync(int id);
}
