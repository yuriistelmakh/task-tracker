using Refit;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardsApi
{
    [Get("/api/boards/my-boards")]
    Task<IApiResponse<IEnumerable<BoardSummaryDto?>>> GetAllAsync();

    [Post("/api/boards")]
    Task<IApiResponse<int>> CreateAsync(CreateBoardRequest request);

    [Get("/api/boards/{id}")]
    public Task<IApiResponse<BoardDetailsDto?>> GetByIdAsync(int id);

    [Post("/api/boards/{id}/reorder")]
    public Task<IApiResponse> ReorderColumnsAsync(int id, [Body] ReorderBoardColumnsRequest request);
}
