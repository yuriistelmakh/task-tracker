using System;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.DTOs.Boards;

public class BoardDetailsDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required string BackgroundColor { get; set; }

    public bool IsArchived { get; set; }

    public required UserSummaryDto Owner { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ColumnSummaryDto> Columns { get; set; } = [];
}
