using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.WebApp.Models.Mapping;

public static class UserMapping
{
    public static UserSummaryModel ToUserSummaryModel(this UserSummaryDto dto) =>
        new()
        {
            Id = dto.Id,
            AvatarUrl = dto.AvatarUrl,
            DisplayName = dto.DisplayName,
            IsDeleted = dto.IsDeleted,
            Tag = dto.Tag
        };
}
