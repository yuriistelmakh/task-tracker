using Refit;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IColumnsApi
{
    [Post("/api/boards/{boardId}/columns")]
    public Task<IApiResponse<int>> CreateAsync(int boardId, [Body] CreateColumnRequest request);

    [Post("/api/boards/{boardId}/columns/{columnId}/reorder")]
    public Task<IApiResponse> ReorderTasksAsync(int boardId, int columnId, [Body] ReorderColumnTasksRequest request);

    [Put("/api/boards/{boardId}/columns/{id}")]
    public Task<IApiResponse> UpdateAsync(int boardId, int id, [Body] UpdateColumnRequest request);

    [Delete("/api/boards/{boardId}/columns/{id}")]
    public Task<IApiResponse> DeleteAsync(int boardId, int id);
}
