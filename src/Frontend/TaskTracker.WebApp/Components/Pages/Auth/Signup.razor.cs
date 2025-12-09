using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Refit;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Services.Abstraction.Interfaces;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Signup
{
    [Inject]
    IAuthApi AuthApi { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }

    SignupModel model = new SignupModel();

    InputType PasswordInputType = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isPasswordVisible = false;

    InputType RepeatPasswordInputType = InputType.Password;
    string RepeatPasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isRepeatPasswordVisible = false;

    private EditContext _editContext;
    private ValidationMessageStore _messageStore;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(model);
        _messageStore = new ValidationMessageStore(_editContext);

        _editContext.OnFieldChanged += (s, e) =>
        {
            _messageStore.Clear(e.FieldIdentifier);

            if (e.FieldIdentifier.FieldName == nameof(SignupModel.Password))
            {
                var repeatField = _editContext.Field(nameof(SignupModel.RepeatPassword));
                _editContext.NotifyFieldChanged(repeatField);
            }
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

    void ToggleRepeatPasswordVisibility()
    {
        if (isRepeatPasswordVisible)
        {
            isRepeatPasswordVisible = false;
            RepeatPasswordInputIcon = Icons.Material.Filled.Visibility;
            RepeatPasswordInputType = InputType.Password;
        }
        else
        {
            isRepeatPasswordVisible = true;
            RepeatPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            RepeatPasswordInputType = InputType.Text;
        }
    }

    async Task OnValidSubmit()
    {
        _messageStore.Clear();
        _editContext.NotifyValidationStateChanged();

        try
        {
            var request = new SignupRequest
            {
                Email = model.Email,
                DisplayName = model.DisplayName,
                Password = model.DisplayName,
                Tag = model.Tag
            };

            await AuthApi.SignupAsync(request);
        }
        catch (ApiException e)
        {
            var error = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Content);

            if (error!["error"] == "EmailTaken")
            {
                _messageStore.Add(() => model.Email, "This email is already taken");
            }
            else if (error!["error"] == "TagTaken")
            {
                _messageStore.Add(() => model.Tag, "This tag is already taken");
            }
            else
            {
                Snackbar.Add($"Something went wrong: {e.Content}", Severity.Error);
            }

            _editContext.NotifyValidationStateChanged();
        }
    }

    class SignupModel
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
}
