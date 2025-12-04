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
            Owner = board.Creator.ToUserSummaryDto(),
            IsArchived = board.IsArchived
        };
}
