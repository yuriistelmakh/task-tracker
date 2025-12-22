using Refit;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface ITasksApi
{
    [Post("/api/tasks")]
    Task<IApiResponse<int>> CreateAsync([Body] CreateTaskRequest request);

    [Patch("/api/tasks/{id}/status")]
    Task<IApiResponse> ChangeStatus(int id, [Body] ChangeTaskStatusRequest request);
}
