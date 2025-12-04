using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities;

[Table("Boards")]
public class Board
{
    [Key]
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }

    public User? Creator { get; set; } 

    public User? Modifier { get; set; }

    public List<BoardColumn> Columns { get; set; } = [];

    public List<BoardMember> Members { get; set; } = [];
}

