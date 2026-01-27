using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebApp.Models.Forms;

public class ChangePasswordModel
{
    public string? OldPassword { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, MinimumLength = 6, ErrorMessage = "Password must contain from 6 to 30 symbols")]
    public string? NewPassword { get; set; }

    [Compare(nameof(NewPassword), ErrorMessage = "Passwords don't match")]
    public string? PasswordRepeat { get; set; }
}
