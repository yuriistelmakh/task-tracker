using System;
using System.Collections.Generic;

namespace TaskTracker.Domain.DTOs;

public class BoardDetailsDto
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public bool IsArchived { get; set; }

    public required UserSummaryDto Owner { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<BoardColumnDto> Columns { get; set; } = [];

    public List<UserSummaryDto> Members { get; set; } = [];
}
