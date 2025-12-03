namespace TaskTracker.Domain.DTOs;

public class BoardColumnDto
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public required string Title { get; set; }

    public int Order { get; set; }
}
