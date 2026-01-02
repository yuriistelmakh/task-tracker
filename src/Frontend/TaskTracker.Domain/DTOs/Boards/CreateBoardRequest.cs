using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Boards;

public class CreateBoardRequest
{
    public required string Title { get; set; }

    public required string DisplayColor { get; set; }

    public BoardVisibility Visibility { get; set; }

    public int CreatedBy { get; set; }
}
