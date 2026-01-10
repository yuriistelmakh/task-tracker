using Refit;
using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardsApi
{
    [Get("/api/boards/my-boards")]
    Task<IApiResponse<PagedResponse<BoardSummaryDto?>>> GetAllAsync(int page, int pageSize);

    [Post("/api/boards")]
    Task<IApiResponse<int>> CreateAsync([Body] CreateBoardRequest request);

    [Get("/api/boards/{id}")]
    public Task<IApiResponse<BoardDetailsDto?>> GetByIdAsync(int id);

    [Put("/api/boards/{id}")]
    public Task<IApiResponse> UpdateAsync(int id, [Body] UpdateBoardRequest request);

    [Post("/api/boards/{id}/reorder")]
    public Task<IApiResponse> ReorderColumnsAsync(int id, [Body] ReorderBoardColumnsRequest request);
}
