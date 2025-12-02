namespace TaskTracker.Domain.DTOs;

public class CreateBoardRequest
{
    public required string Title { get; set; }

    public string Description { get; set; } = string.Empty;
}
