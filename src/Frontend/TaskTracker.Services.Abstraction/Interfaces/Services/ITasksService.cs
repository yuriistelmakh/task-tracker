using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface ITasksService
{
    Task<Result<TaskDetailsDto>> GetByIdAsync(int boardId, int id);
    Task<Result<IEnumerable<TaskSummaryDto>>> SearchAsync(int boardId, string? prompt, int page, int pageSize);
    Task<Result> ChangeStatusAsync(int boardId, int id, ChangeTaskStatusRequest request);
    Task<Result<int>> CreateAsync(int boardId, CreateTaskRequest request);
    Task<Result> DeleteAsync(int boardId, int id);
    Task<Result> UpdateAsync(int boardId, int id, UpdateTaskRequest request);
}
