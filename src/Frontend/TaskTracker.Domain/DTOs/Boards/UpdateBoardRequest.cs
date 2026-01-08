namespace TaskTracker.Domain.DTOs.Boards;

public class UpdateBoardRequest
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string BackgroundColor { get; set; }

    public int UpdatedBy { get; set; }

    public bool IsArchived { get; set; }
}
