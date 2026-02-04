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

    public async Task<Result<int>> CreateAsync(int boardId, CreateTaskRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result<int>.Failure("User id was not found");
        }

        request.CreatedBy = userId.Value;

        var response = await _tasksApi.CreateAsync(boardId, request);

        return response.IsSuccessful
            ? Result<int>.Success(response.Content)
            : Result<int>.Failure(response.Error.Message);
    }

    public async Task<Result> ChangeStatusAsync(int boardId, int id, ChangeTaskStatusRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result.Failure("User id was not found");
        }

        request.UpdatedBy = userId.Value;

        var response = await _tasksApi.ChangeStatusAsync(boardId, id, request);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }

    public async Task<Result<TaskDetailsDto>> GetByIdAsync(int boardId, int id)
    {
        var response = await _tasksApi.GetByIdAsync(boardId, id);

        return response.IsSuccessful
            ? Result<TaskDetailsDto>.Success(response.Content)
            : Result<TaskDetailsDto>.Failure(response.Error.Message);
    }
    
    public async Task<Result<IEnumerable<TaskSummaryDto>>> SearchAsync(int boardId, string? prompt, int page, int pageSize)
    {
        var response = await _tasksApi.SearchAsync(boardId, prompt, page, pageSize);

        return response.IsSuccessful
            ? Result<IEnumerable<TaskSummaryDto>>.Success(response.Content)
            : Result<IEnumerable<TaskSummaryDto>>.Failure(response.Error.Message);
    }

    public async Task<Result> UpdateAsync(int boardId, int id, UpdateTaskRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result.Failure("User id was not found");
        }

        request.UpdatedBy = userId.Value;

        var response = await _tasksApi.UpdateAsync(boardId, id, request);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }

    public async Task<Result> DeleteAsync(int boardId, int id)
    {
        var response = await _tasksApi.DeleteAsync(boardId, id);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }
}
