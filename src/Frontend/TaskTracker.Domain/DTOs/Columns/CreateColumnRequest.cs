namespace TaskTracker.Domain.DTOs.Columns;

public class CreateColumnRequest
{
    public int BoardId { get; set; }

    public required string Title { get; set; }

    public int Order { get; set; }

    public int CreatedBy { get; set; }
}
