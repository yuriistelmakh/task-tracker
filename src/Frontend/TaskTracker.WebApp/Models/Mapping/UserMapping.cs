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
            Tag = dto.Tag
        };

    public static MemberModel ToMemberModel(this MemberSummaryDto memberSummaryDto) =>
        new()
        {
            DisplayName = memberSummaryDto.DisplayName,
            AvatarUrl = memberSummaryDto.AvatarUrl,
            Id = memberSummaryDto.Id,
            Role = memberSummaryDto.Role,
            Tag = memberSummaryDto.Tag,
        };
}
