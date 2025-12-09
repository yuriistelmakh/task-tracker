using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Refit;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Services.Abstraction.Interfaces;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Login
{
    [Inject]
    IAuthApi AuthApi { get; set; }

    [Inject]
    ISnackbar SnackBar { get; set; }

    LoginModel model = new LoginModel();

    InputType PasswordInputType = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isPasswordVisible = false;

    private EditContext _editContext;
    private ValidationMessageStore _messageStore;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(model);
        _messageStore = new ValidationMessageStore(_editContext);

        _editContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
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

        try
        {
            var request = new LoginRequest { Password = model.Password };

            if (model.Login.Contains("@"))
                request.Email = model.Login;
            else
                request.Tag = model.Login;

            await AuthApi.LoginAsync(request);
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                _messageStore.Add(() => model.Login, "User was not found");
            }
            else if (e.StatusCode == HttpStatusCode.Unauthorized)
            {
                _messageStore.Add(() => model.Password, "Incorrect password");
            }
            else
            {
                SnackBar.Add($"Something went wrong: {e.Content}", Severity.Error);
            }

            _editContext.NotifyValidationStateChanged();
        }
    }

    class LoginModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; } = string.Empty;
    }
}
