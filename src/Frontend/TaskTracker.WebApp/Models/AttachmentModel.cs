using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Enums;
using TaskTracker.WebApp.Models.Users;

namespace TaskTracker.WebApp.Models;

public class AttachmentModel
{
    public int Id { get; set; }

    public UserSummaryModel CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public required string FileUrl { get; set; }

    public required string Name { get; set; }

    public double SizeKB { get; set; }

    public FileType Type { get; set; }
}
