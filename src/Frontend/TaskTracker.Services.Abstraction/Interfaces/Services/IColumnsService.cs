using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IColumnsService
{
    Task<Result<int>> CreateAsync(CreateColumnRequest request);

    Task<Result> ReorderAsync(int columndId, ReorderColumnTasksRequest request);
}
