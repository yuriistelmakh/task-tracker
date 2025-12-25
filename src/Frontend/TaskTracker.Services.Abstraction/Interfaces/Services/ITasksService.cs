using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface ITasksService
{
    Task<Result<int>> CreateAsync(CreateTaskRequest request);

    Task<Result> ChangeStatusAsync(int id, ChangeTaskStatusRequest request);

    Task<Result<TaskDetailsDto>> GetByIdAsync(int id);

    Task<Result> UpdateAsync(int id, UpdateTaskRequest request);

    Task<Result> DeleteAsync(int id);
}
