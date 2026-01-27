using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models.Forms;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Login
{
    [Inject] private ISnackbar SnackBar { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IAuthService AuthService { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "returnUrl")]
    public string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery(Name = "errorCode")]
    public int? ErrorCode { get; set; }

    [SupplyParameterFromQuery(Name = "loginAttempt")]
    public string? LoginAttempt { get; set; }

    private readonly LoginModel model = new();
    private bool _isProcessing;

    private bool _isLoginError;
    private string? _loginErrorText;

    private bool _isPasswordError;
    private string? _passwordErrorText;

    private InputType PasswordInputType = InputType.Password;
    private string PasswordInputIcon = Icons.Material.Filled.Visibility;
    private bool isPasswordVisible = false;

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

    private async Task HandleLogin()
    {
        _isProcessing = true;
        ClearErrors();

        try
        {
            var request = new LoginRequest
            {
                Password = model.Password
            };

            if (model.Login.Contains('@'))
            {
                request.Email = model.Login;
            }
            else
            {
                request.Tag = model.Login;
            }

            var result = await AuthService.LoginAsync(request);

            if (result == AuthErrorType.None)
            {
                Navigation.NavigateTo(ReturnUrl ?? "/");
            }
            else
            {
                HandleAuthError(result);
            }
        }
        catch (Exception)
        {
            SnackBar.Add("An unexpected error occurred. Please try again.", Severity.Error);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void HandleAuthError(AuthErrorType errorType)
    {
        switch (errorType)
        {
            case AuthErrorType.UserNotFound:
                _isLoginError = true;
                _loginErrorText = "User was not found";
                break;
            case AuthErrorType.InvalidPassword:
                _isPasswordError = true;
                _passwordErrorText = "Wrong password";
                break;
            default:
                SnackBar.Add("Server error. Try again later.", Severity.Error);
                break;
        }
    }

    private void ClearErrors()
    {
        _isLoginError = false;
        _isPasswordError = false;
        _loginErrorText = null;
        _passwordErrorText = null;
    }

    private void TogglePasswordVisibility()
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