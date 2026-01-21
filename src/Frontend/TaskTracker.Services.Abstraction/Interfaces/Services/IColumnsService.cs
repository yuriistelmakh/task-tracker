using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IColumnsService
{
    Task<Result<int>> CreateAsync(int boardId, CreateColumnRequest request);
    Task<Result> DeleteAsync(int boardId, int id);
    Task<Result> ReorderAsync(int boardId, int columndId, ReorderColumnTasksRequest request);
    Task<Result> UpdateAsync(int boardId, int columnId, UpdateColumnRequest request);
}
