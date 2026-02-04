using Microsoft.AspNetCore.Identity.UI.Services;
using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Notifications;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public class UsersService : IUsersService
{
    private readonly IUsersApi _usersApi;

    public UsersService(IUsersApi usersApi)
    {
        _usersApi = usersApi;
    }

    public async Task<Result<UserDetailsDto>> GetByIdAsync(int id)
    {
        var result = await _usersApi.GetByIdAsync(id);

        return result.IsSuccessful
            ? Result<UserDetailsDto>.Success(result.Content)
            : Result<UserDetailsDto>.Failure(result.Error.Message);
    }

    public async Task<Result> UpdateAsync(int id, UpdateUserRequest userUpdate)
    {
        var result = await _usersApi.UpdateAsync(id, userUpdate);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }

    public async Task<Result<IEnumerable<UserSummaryDto>>> SearchAsync(string prompt, int pageSize)
    {
        var result = await _usersApi.SearchAsync(prompt, pageSize);

        return result.IsSuccessful
            ? Result<IEnumerable<UserSummaryDto>>.Success(result.Content)
            : Result<IEnumerable<UserSummaryDto>>.Failure(result.Error.Message);
    }

    public async Task<Result<IEnumerable<NotificationDto>>> GetUnreadNotifications(int userId)
    {
        var result = await _usersApi.GetUnreadNotificationsAsync(userId);

        return result.IsSuccessful
            ? Result<IEnumerable<NotificationDto>>.Success(result.Content)
            : Result<IEnumerable<NotificationDto>>.Failure(result.Error.Message);
    }
}
