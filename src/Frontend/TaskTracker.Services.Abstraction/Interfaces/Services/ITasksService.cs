using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface ITasksService
{
    Task<Result<int>> CreateAsync(CreateTaskRequest request);
}
