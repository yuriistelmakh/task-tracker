using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Signup
{
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IAuthService AuthService { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "email")]
    public string? EmailParam { get; set; }

    [SupplyParameterFromQuery(Name = "tag")]
    public string? TagParam { get; set; }

    [SupplyParameterFromQuery(Name = "displayName")]
    public string? DisplayNameParam { get; set; }

    private readonly SignupModel model = new();
    private EditContext _editContext = default!;
    private ValidationMessageStore _messageStore = default!;
    private bool _isProcessing;

    InputType PasswordInputType = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isPasswordVisible = false;

    InputType RepeatPasswordInputType = InputType.Password;
    string RepeatPasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isRepeatPasswordVisible = false;

    protected override void OnInitialized()
    {
        if (!string.IsNullOrEmpty(EmailParam)) model.Email = EmailParam;
        if (!string.IsNullOrEmpty(TagParam)) model.Tag = TagParam;
        if (!string.IsNullOrEmpty(DisplayNameParam)) model.DisplayName = DisplayNameParam;

        _editContext = new EditContext(model);
        _messageStore = new ValidationMessageStore(_editContext);

        _editContext.OnFieldChanged += (s, e) =>
        {
            _messageStore.Clear(e.FieldIdentifier);
            _editContext.NotifyValidationStateChanged();
        };
    }

    private async Task HandleSignup()
    {
        _isProcessing = true;
        _messageStore.Clear();
        
        try
        {
            var request = new SignupRequest
            {
                Email = model.Email,
                Password = model.Password,
                DisplayName = model.DisplayName,
                Tag = model.Tag
            };

            var result = await AuthService.SignupAsync(request);

            if (result == AuthErrorType.None)
            {
                Snackbar.Add("Account created successfully!", Severity.Success);
                
                await Task.Delay(50);
                
                Navigation.NavigateTo("/");
            }
            else
            {
                HandleAuthError(result);
            }
        }
        catch (Exception)
        {
            Snackbar.Add("An unexpected error occurred. Please try again.", Severity.Error);
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
            case AuthErrorType.EmailTaken:
                AddErrorToField(nameof(model.Email), "This email is already taken");
                break;
                
            case AuthErrorType.TagTaken:
                AddErrorToField(nameof(model.Tag), "This tag is already taken");
                break;
                
            case AuthErrorType.None:
                break;
                
            default:
                Snackbar.Add("Registration failed. Please try again later.", Severity.Error);
                break;
        }
    }

    private void AddErrorToField(string fieldName, string errorMessage)
    {
        var field = _editContext.Field(fieldName);
        _messageStore.Add(field, errorMessage);
        _editContext.NotifyValidationStateChanged();
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
}