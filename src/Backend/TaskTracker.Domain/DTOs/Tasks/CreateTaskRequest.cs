namespace TaskTracker.Domain.DTOs.Tasks;

public class CreateTaskRequest
{
    public int ColumnId { get; set; }

    public required string Title { get; set; }

    public int CreatedBy { get; set; }
}
