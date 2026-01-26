using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Mapping;

public static class MemberMapping
{
    public static MemberSummaryDto ToMemberSummaryDto(this BoardMember member, BoardRole role) =>
        new()
        {
            Id = member.User.Id,
            AvatarUrl = member.User.AvatarUrl,
            DisplayName = member.User.DisplayName,
            Email = member.User.Email,
            Role = role,
            Tag = member.User.Tag,
            JoinedAt = member.JoinedAt
        };
}
