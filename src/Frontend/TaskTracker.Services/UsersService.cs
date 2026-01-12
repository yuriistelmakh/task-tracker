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
