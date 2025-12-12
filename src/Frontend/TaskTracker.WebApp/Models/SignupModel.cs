using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebApp.Models;

public class SignupModel
{
    [Required(ErrorMessage = "This field is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is required")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must contain from 6 to 30 symbols")]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
    public string RepeatPassword { get; set; } = string.Empty;

    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Only latin characters and numbers are allowed")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Tag must contain from 6 to 30 symbols")]
    [Required(ErrorMessage = "This field is required")]
    public string Tag { get; set; } = string.Empty;

    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must contain from 3 to 30 symbols")]
    [Required(ErrorMessage = "This field is required")]
    public string DisplayName { get; set; } = string.Empty;
}