using System.ComponentModel.DataAnnotations;

namespace TaskTracker.WebApp.Models;

public class LoginModel
{
    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}