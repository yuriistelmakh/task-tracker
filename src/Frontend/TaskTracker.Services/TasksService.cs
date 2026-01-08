using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class TasksService : ITasksService
{
    private readonly ITasksApi _tasksApi;
    private readonly ICurrentUserService _userService;

    public TasksService(ITasksApi tasksApi, ICurrentUserService userService)
    {
        _tasksApi = tasksApi;
        _userService = userService;
    }

    public async Task<Result<int>> CreateAsync(CreateTaskRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result<int>.Failure("User id was not found");
        }

        request.CreatedBy = userId.Value;

        var response = await _tasksApi.CreateAsync(request);

        return response.IsSuccessful
            ? Result<int>.Success(response.Content)
            : Result<int>.Failure(response.Error.Message);
    }

    public async Task<Result> ChangeStatusAsync(int id, ChangeTaskStatusRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result.Failure("User id was not found");
        }

        request.UpdatedBy = userId.Value;

        var response = await _tasksApi.ChangeStatusAsync(id, request);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }

    public async Task<Result<TaskDetailsDto>> GetByIdAsync(int id)
    {
        var response = await _tasksApi.GetByIdAsync(id);

        return response.IsSuccessful
            ? Result<TaskDetailsDto>.Success(response.Content)
            : Result<TaskDetailsDto>.Failure(response.Error.Message);
    }

    public async Task<Result> UpdateAsync(int id, UpdateTaskRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result.Failure("User id was not found");
        }

        request.UpdatedBy = userId.Value;

        var response = await _tasksApi.UpdateAsync(id, request);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var response = await _tasksApi.DeleteAsync(id);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }
}
