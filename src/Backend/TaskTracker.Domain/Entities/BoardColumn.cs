using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace TaskTracker.Domain.Entities;

[Table("Columns")]
public class BoardColumn
{
    [Key]
    public int Id { get; set; }

    public int BoardId { get; set; }

    public required string Title { get; set; }

    public int Order { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }

    [Editable(false)]
    public List<BoardTask> Tasks { get; set; } = [];
}
