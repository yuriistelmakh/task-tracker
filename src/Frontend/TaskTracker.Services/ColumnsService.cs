using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class ColumnsService : IColumnsService
{
    private readonly ICurrentUserService _userService;
    private readonly IColumnsApi _columnsApi;

    public ColumnsService(ICurrentUserService userService, IColumnsApi columnsApi)
    {
        _userService = userService;
        _columnsApi = columnsApi;
    }

    public async Task<Result<int>> CreateAsync(CreateColumnRequest request)
    {
        var userId = await _userService.GetUserId();

        if (userId is null)
        {
            return Result<int>.Failure("User id was not found");
        }

        request.CreatedBy = userId.Value;

        var result = await _columnsApi.CreateAsync(request);

        return result.IsSuccessful
            ? Result<int>.Success(result.Content)
            : Result<int>.Failure(result.Error.Message);
    }

    public async Task<Result> ReorderAsync(int columndId, ReorderColumnTasksRequest request)
    {
        var response = await _columnsApi.ReorderAsync(columndId, request);

        return response.IsSuccessful
            ? Result.Success()
            : Result.Failure(response.Error.Message);
    }
}