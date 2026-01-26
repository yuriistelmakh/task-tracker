using Refit;
using TaskTracker.Domain.DTOs.Notifications;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IUsersApi
{
    [Get("/api/users")]
    public Task<IApiResponse<IEnumerable<UserSummaryDto>>> SearchAsync(string prompt, int pageSize);

    [Get("/api/users/{id}")]
    public Task<IApiResponse<UserDetailsDto>> GetByIdAsync(int id);

    [Put("/api/users/{id}")]
    public Task<IApiResponse> UpdateAsync(int id, UpdateUserRequest request);

    [Get("/api/users/{id}/notifications")]
    public Task<IApiResponse<IEnumerable<NotificationDto>>> GetUnreadNotificationsAsync(int id);
}
