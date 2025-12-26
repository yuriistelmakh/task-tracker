using Refit;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IColumnsApi
{
    [Post("/api/columns")]
    public Task<IApiResponse<int>> CreateAsync([Body] CreateColumnRequest request);

    [Post("/api/columns/{columnId}/reorder")]
    public Task<IApiResponse> ReorderAsync(int columnId, [Body] ReorderColumnTasksRequest request);

    [Put("/api/columns/{id}")]
    public Task<IApiResponse> UpdateAsync(int id, [Body] UpdateColumnRequest request);

    [Delete("/api/columns/{id}")]
    public Task<IApiResponse> DeleteAsync(int id);
}
