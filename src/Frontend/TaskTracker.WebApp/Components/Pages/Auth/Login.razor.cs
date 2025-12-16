using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Domain.Enums;
using TaskTracker.WebApp.Models;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Login
{
    [Inject] public ISnackbar SnackBar { private get; set; } = default!;

    [SupplyParameterFromQuery(Name = "returnUrl")]
    public string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery(Name = "errorCode")]
    public int? ErrorCode { get; set; }

    [SupplyParameterFromQuery(Name = "loginAttempt")]
    public string? LoginAttempt { get; set; }

    private readonly LoginModel model = new();

    private bool _isLoginError;
    private string? _loginErrorText;

    private bool _isPasswordError;
    private string? _passwordErrorText;

    InputType PasswordInputType = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isPasswordVisible = false;

    protected override void OnInitialized()
    {
        if (!string.IsNullOrEmpty(LoginAttempt))
        {
            model.Login = LoginAttempt;
        }

        if (ErrorCode.HasValue)
        {
            HandleAuthError((AuthErrorType)ErrorCode.Value);
        }
    }

    private void HandleAuthError(AuthErrorType errorType)
    {
        switch (errorType)
        {
            case AuthErrorType.UserNotFound:
            {
                _isLoginError = true;
                _loginErrorText = "User was not found";
                break;
            }
            case AuthErrorType.InvalidPassword:
            {
                _isPasswordError = true;
                _passwordErrorText = "Wrong password";
                break;
            }
            case AuthErrorType.Unknown:
            default:
            {
                SnackBar.Add("Server error. Try again later.", Severity.Error);
                break;
            }
        }
    }

    void TogglePasswordVisibility()
    {
        if (isPasswordVisible)
        {
            isPasswordVisible = false;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInputType = InputType.Password;
        }
        else
        {
            isPasswordVisible = true;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInputType = InputType.Text;
        }
    }
}