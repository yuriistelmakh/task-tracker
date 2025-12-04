namespace TaskTracker.Domain.DTOs.Boards;

public class CreateBoardRequest
{
    public required string Title { get; set; }

    public string Description { get; set; } = string.Empty;

    public int CreatedBy { get; set; }
}
