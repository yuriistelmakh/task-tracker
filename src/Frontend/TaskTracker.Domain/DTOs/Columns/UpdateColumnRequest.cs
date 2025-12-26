namespace TaskTracker.Domain.DTOs.Columns;

public class UpdateColumnRequest
{
    public required string Title { get; set; }

    public int Order { get; set; }

    public int UpdatedBy { get; set; }
}
