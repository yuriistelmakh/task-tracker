using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using TaskTracker.Domain.Enums;
using TaskTracker.WebApp.Models;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class Signup
{
    [Inject]
    public ISnackbar Snackbar { private get; set; } = default!;

    [Inject]
    public IJSRuntime JS { private get; set; } = default!;

    [SupplyParameterFromQuery(Name = "errorCode")]
    public int? ErrorCode { get; set; }

    [SupplyParameterFromQuery(Name = "customError")]
    public string? CustomError { get; set; }

    [SupplyParameterFromQuery(Name = "email")]
    public string? EmailParam { get; set; }

    [SupplyParameterFromQuery(Name = "tag")]
    public string? TagParam { get; set; }

    [SupplyParameterFromQuery(Name = "displayName")]
    public string? DisplayNameParam { get; set; }

    private readonly SignupModel model = new();
    private EditContext _editContext = default!;
    private ValidationMessageStore _messageStore = default!;

    InputType PasswordInputType = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isPasswordVisible = false;

    InputType RepeatPasswordInputType = InputType.Password;
    string RepeatPasswordInputIcon = Icons.Material.Filled.Visibility;
    bool isRepeatPasswordVisible = false;

    protected override void OnInitialized()
    {
        if (!string.IsNullOrEmpty(EmailParam))
        {
            model.Email = EmailParam;
        }
        if (!string.IsNullOrEmpty(TagParam))
        {
            model.Tag = TagParam;
        }
        if (!string.IsNullOrEmpty(DisplayNameParam))
        {
            model.DisplayName = DisplayNameParam;
        }

        _editContext = new EditContext(model);
        _messageStore = new ValidationMessageStore(_editContext);

        _editContext.OnFieldChanged += (s, e) =>
        {
            _messageStore.Clear(e.FieldIdentifier);
            _editContext.NotifyValidationStateChanged();
        };

        if (!string.IsNullOrEmpty(CustomError))
        {
            AddErrorToField(nameof(model.Password), CustomError);
            Snackbar.Add(CustomError, Severity.Error);
        }

        if (ErrorCode.HasValue)
        {
            HandleAuthError((AuthErrorType)ErrorCode.Value);
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            if (ErrorCode.HasValue || !string.IsNullOrEmpty(CustomError))
            {
                _editContext.NotifyValidationStateChanged();
                StateHasChanged();
            }
        }
        base.OnAfterRender(firstRender);
    }

    private void HandleAuthError(AuthErrorType errorType)
    {
        switch (errorType)
        {
            case AuthErrorType.EmailTaken:
            {
                AddErrorToField(nameof(model.Email), "This email is already taken");
                break;
            }
            case AuthErrorType.TagTaken:
            {
                AddErrorToField(nameof(model.Tag), "This tag is already taken");
                break;
            }
            case AuthErrorType.None:
            {
                break;
            }
            default:
            {
                Snackbar.Add("Something went wrong. Please try again later.", Severity.Error);
                break;
            }
        }
    }

    private void AddErrorToField(string fieldName, string errorMessage)
    {
        var field = _editContext.Field(fieldName);
        _messageStore.Add(field, errorMessage);
        _editContext.NotifyValidationStateChanged();
    }

    private async Task OnValidSubmit()
    {
        await JS.InvokeVoidAsync("submitFormById", "signup-form");
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
