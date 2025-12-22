using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.WebApp.Models.Mapping;

public static class BoardMapping
{
    public static BoardModel ToBoardModel(this BoardSummaryDto dto) =>
        new()
        {
            Id = dto.Id,
            Title = dto.Title,
            OwnerName = dto.Owner.DisplayName,
            MembersCount = dto.MembersCount,
            TasksCount = dto.TasksCount,
            IsArchived = dto.IsArchived,
            OwnerIconUrl = dto.Owner.AvatarUrl
        };
}
