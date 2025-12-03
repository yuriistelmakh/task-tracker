namespace TaskTracker.Domain.DTOs;

public class BoardSummaryDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public bool IsArchived { get; set; }

    public required UserSummaryDto Owner { get; set; }
}
