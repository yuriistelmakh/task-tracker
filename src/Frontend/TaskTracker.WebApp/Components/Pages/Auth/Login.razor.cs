using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Login
{
    [Inject]
    public ISnackbar SnackBar { private get; set; } = default!;

    [Inject]
    public IAuthService AuthService { private get; set; } = default!;

    private readonly LoginModel model = new();

    InputType PasswordInputType = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isPasswordVisible = false;

    private EditContext _editContext = default!;
    private ValidationMessageStore _messageStore = default!;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(model);
        _messageStore = new ValidationMessageStore(_editContext);

        _editContext.OnFieldChanged += (s, e) =>
        {
            _messageStore.Clear(e.FieldIdentifier);
            _editContext.NotifyValidationStateChanged();
        };
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

    async Task OnValidSubmit()
    {
        _messageStore.Clear();
        _editContext.NotifyValidationStateChanged();

        var request = new LoginRequest
        {
            Password = model.Password
        };

        if (model.Login.Contains('@'))
            request.Email = model.Login;
        else
            request.Tag = model.Login;

        var result = await AuthService.LoginAsync(request);

        if (result.IsSuccess)
        {
            SnackBar.Add("You authorized successfully. Now wait for home page implementation lmao",
                Severity.Success);
            return;
        }

        switch (result.ErrorType)
        {
            case AuthErrorType.UserNotFound:
                _messageStore.Add(
                    _editContext.Field(nameof(LoginModel.Login)),
                    "User was not found");
                break;

            case AuthErrorType.InvalidPassword:
                _messageStore.Add(
                    _editContext.Field(nameof(LoginModel.Password)),
                    "Incorrect password");
                break;

            default:
                SnackBar.Add("Something went wrong. Please try again later.", Severity.Error);
                break;
        }

        _editContext.NotifyValidationStateChanged();
    }

    class LoginModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; } = string.Empty;
    }
}
