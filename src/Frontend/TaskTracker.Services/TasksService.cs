using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class TasksService : ITasksService
{
    private readonly ITasksApi _tasksApi;

    private readonly ISessionCacheService _cache;

    public TasksService(ITasksApi tasksApi, ISessionCacheService cache)
    {
        _tasksApi = tasksApi;
        _cache = cache;
    }

    public async Task<Result<int>> CreateAsync(CreateTaskRequest request)
    {
        var userData = _cache.GetSessionData(_cache.CurrentSessionId);

        if (userData is null)
        {
            return Result<int>.Failure("User data was not cached");
        }

        request.CreatedBy = userData.Id;

        var response = await _tasksApi.CreateAsync(request);

        if (!response.IsSuccessful)
        {
            return Result<int>.Failure(response.Error.Message);
        }

        return Result<int>.Success(response.Content);
    }
}
