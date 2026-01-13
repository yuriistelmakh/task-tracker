using Refit;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface ITasksApi
{
    [Get("/api/boards/{boardId}/tasks/{id}")]
    Task<IApiResponse<TaskDetailsDto>> GetByIdAsync(int boardId, int id);

    [Get("/api/boards/{boardId}/tasks/search?prompt={prompt}&page={page}&pageSize={pageSize}")]
    Task<IApiResponse<IEnumerable<TaskSummaryDto>>> SearchAsync(int boardId, string? prompt, int page, int pageSize);

    [Post("/api/boards/{boardId}/tasks")]
    Task<IApiResponse<int>> CreateAsync(int boardId, [Body] CreateTaskRequest request);

    [Patch("/api/boards/{boardId}/tasks/{id}/status")]
    Task<IApiResponse> ChangeStatusAsync(int boardId, int id, [Body] ChangeTaskStatusRequest request);

    [Put("/api/boards/{boardId}/tasks/{id}")]
    Task<IApiResponse> UpdateAsync(int boardId, int id, [Body] UpdateTaskRequest request);

    [Delete("/api/boards/{boardId}/tasks/{id}")]
    Task<IApiResponse> DeleteAsync(int boardId, int id);
}
