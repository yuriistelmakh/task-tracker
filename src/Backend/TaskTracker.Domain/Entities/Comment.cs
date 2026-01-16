using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Domain.Entities;

[Table("comments")]
public class Comment
{
    [Key]
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int TaskId { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public User Creator { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
}
