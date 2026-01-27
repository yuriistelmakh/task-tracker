using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Notifications;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IUsersService
{
    Task<Result> ChangePasswordAsync(int userId, ChangePasswordRequest changePasswordRequest);
    Task<Result<UserDetailsDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<NotificationDto>>> GetUnreadNotifications(int userId);
    Task<Result<IEnumerable<UserSummaryDto>>> SearchAsync(string prompt, int pageSize);
    Task<Result> UpdateAsync(int id, UpdateUserRequest userUpdate);
    Task<Result> DeleteAsync(int userId);
}
