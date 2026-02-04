using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Mapping;

public static class UserMapping
{
    public static UserSummaryDto ToUserSummaryDto(this User user) =>
        new() 
        { 
            Id = user.Id,
            DisplayName = user.DisplayName,
            Tag = user.Tag,
            AvatarUrl = user.AvatarUrl,
        };

    public static UserDetailsDto ToUserDetailsDto(this User user) =>
        new() 
        { 
            Id = user.Id,
            DisplayName = user.DisplayName,
            Tag = user.Tag,
            AvatarUrl = user.AvatarUrl,
            Email = user.Email
        };
}
