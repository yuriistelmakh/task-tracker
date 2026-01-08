using Refit;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface ITasksApi
{
    [Get("/api/tasks/{id}")]
    Task<IApiResponse<TaskDetailsDto>> GetByIdAsync(int id);

    [Post("/api/tasks")]
    Task<IApiResponse<int>> CreateAsync([Body] CreateTaskRequest request);

    [Patch("/api/tasks/{id}/status")]
    Task<IApiResponse> ChangeStatusAsync(int id, [Body] ChangeTaskStatusRequest request);

    [Put("/api/tasks/{id}")]
    Task<IApiResponse> UpdateAsync(int id, [Body] UpdateTaskRequest request);

    [Delete("/api/tasks/{id}")]
    Task<IApiResponse> DeleteAsync(int id);
}
