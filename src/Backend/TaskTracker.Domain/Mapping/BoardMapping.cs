using System.Linq;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class BoardMapping
{
    public static BoardDetailsDto ToBoardDetailsDto(this Board board) =>
        new()
        {
            Id = board.Id,
            Title = board.Title,
            Owner = board.Creator.ToUserSummaryDto(),
            DisplayColor = board.DisplayColor,
            CreatedAt = board.CreatedAt,
            Description= board.Description,
            IsArchived = board.IsArchived,
            Columns = board.Columns.Select(c => c.ToColumnSummaryDto()).ToList()
        };

    public static BoardSummaryDto ToBoardSummaryDto(this Board board) =>
        new()
        {
            Id = board.Id,
            Title = board.Title,
            DisplayColor = board.DisplayColor,
            Members = board.Members.Select(m => m.ToUserSummaryDto()).ToList(),
            IsArchived = board.IsArchived,
            MembersCount = board.Members.Count,
            TasksCount = board.Columns.Sum(c => c.Tasks.Count)
        };
}
