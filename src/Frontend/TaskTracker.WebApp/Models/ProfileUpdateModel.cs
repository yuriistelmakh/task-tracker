namespace TaskTracker.WebApp.Models;

using System.ComponentModel.DataAnnotations;

public class ProfileUpdateModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Only latin characters and numbers are allowed")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Tag must contain from 6 to 30 symbols")]
    [Required(ErrorMessage = "Tag is required")]
    public string Tag { get; set; } = string.Empty;

    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must contain from 3 to 30 symbols")]
    [Required(ErrorMessage = "Name is required")]
    public string DisplayName { get; set; } = string.Empty;
}
