using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebApp.Models;

public class LoginModel
{
    [Required(ErrorMessage = "This field is required")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "This field is required")]
    public string Password { get; set; } = string.Empty;
}