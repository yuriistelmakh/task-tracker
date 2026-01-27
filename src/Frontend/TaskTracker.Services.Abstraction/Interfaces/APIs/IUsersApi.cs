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
    public Task<IApiResponse> UpdateAsync(int id, [Body] UpdateUserRequest request);

    [Get("/api/users/{id}/notifications")]
    public Task<IApiResponse<IEnumerable<NotificationDto>>> GetUnreadNotificationsAsync(int id);

    [Put("/api/users/{id}/change-password")]
    public Task<IApiResponse> ChangePasswordAsync(int id, [Body] ChangePasswordRequest request);

    [Multipart]
    [Put("/api/users/{id}/avatar")]
    public Task<IApiResponse> UploadAvatarAsync(int id, StreamPart avatar);

    [Delete("/api/users/{id}")]
    public Task<IApiResponse> DeleteAsync(int id);
}
