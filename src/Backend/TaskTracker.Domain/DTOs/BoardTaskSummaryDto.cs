using System;

namespace TaskTracker.Domain.DTOs;

public class BoardTaskSummaryDto
{
    public int Id { get; set; }

    public int ColumndId { get; set; }

    public required string Title { get; set; }

    public int Prioruty { get; set; } = 1;

    public int Position { get; set; }

    public int? AssigneeId { get; set; }

}
