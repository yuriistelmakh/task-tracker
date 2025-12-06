namespace TaskTracker.Domain.DTOs.Boards;

public class UpdateBoardRequest
{
    public required string Title { get; set; }

    public string Description { get; set; } = string.Empty;

    public int UpdatedBy { get; set; }

    public bool IsArchived { get; set; }
}
