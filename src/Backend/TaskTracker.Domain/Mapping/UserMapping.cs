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
            IsDeleted = user.IsDeleted
        };

    public static MemberSummaryDto ToMemberSummaryDto(this User user, BoardRole role) =>
        new()
        {
            Id = user.Id,
            AvatarUrl = user.AvatarUrl,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Role = role,
            Tag = user.Tag,
        };
}
