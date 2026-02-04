using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models.Mapping;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Users;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.WebApp.Models.Forms;
using TaskTracker.WebApp.Components.Shared;

namespace TaskTracker.WebApp.Components.Pages.Profile;

public partial class UserProfile
{
    [Parameter]
    public int UserId { get; set; }

    [Inject]
    public IUsersService UsersService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    [Inject]
    public NavigationManager Navigation { get; private set; } = default!;

    [Inject]
    public IAuthService AuthService { get; private set; } = default!;

    private UserDetailsModel _user = new() { Email = string.Empty, DisplayName = string.Empty, Tag = string.Empty };

    private ProfileUpdateModel _profileUpdate = new();
    private ChangePasswordModel _changePassword = new();

    private bool _isEditingMode = false;

    private Variant _inputVariant => _isEditingMode ? Variant.Outlined : Variant.Text;

    private int _activeTab = 1;

    private InputType _oldPasswordInputType = InputType.Password;
    private string _oldPasswordInputIcon = Icons.Material.Filled.Visibility;
    private bool _isOldPasswordVisible = false;

    private InputType _newPasswordInputType = InputType.Password;
    private string _newPasswordInputIcon = Icons.Material.Filled.Visibility;
    private bool _isNewPasswordVisible = false;

    private InputType _repeatPasswordInputType = InputType.Password;
    private string _repeatPasswordInputIcon = Icons.Material.Filled.Visibility;
    private bool _isRepeatPasswordVisible = false;

    protected override async Task OnInitializedAsync()
    {
        var result = await UsersService.GetByIdAsync(UserId);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error getting user: {result.ErrorMessage}");
            return;
        }

        _user = result.Value!.ToUserDetailsModel();

        _profileUpdate = new ProfileUpdateModel
        {
            DisplayName = _user.DisplayName,
            Tag = _user.Tag,
            Email = _user.Email
        };
    }

    private void OnEditClicked()
    {
        _isEditingMode = true;
    }

    private void OnCancelClicked()
    {
        _isEditingMode = false;

        _profileUpdate = new ProfileUpdateModel
        {
            DisplayName = _user.DisplayName,
            Tag = _user.Tag,
            Email = _user.Email
        };
    }

    private async Task OnUpdateUserValidSubmit()
    {
        var request = new UpdateUserRequest
        {
            DisplayName = _profileUpdate.DisplayName,
            Tag = _profileUpdate.Tag,
            Email = _profileUpdate.Email
        };

        var result = await UsersService.UpdateAsync(UserId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add(result!.ErrorMessage, Severity.Error);
            return;
        }

        _isEditingMode = false;
    }

    private async Task OnChangePasswordValidSubmit()
    {
        var request = new ChangePasswordRequest
        {
            OldPassword = _changePassword.OldPassword,
            NewPassword = _changePassword.NewPassword
        };

        var result = await UsersService.ChangePasswordAsync(UserId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add(result.ErrorMessage!, Severity.Error);
            return;
        }

        Snackbar.Add("Password changed successfully", Severity.Success);
    }

    private void ToggleOldPasswordVisibility()
    {
        if (_isOldPasswordVisible)
        {
            _isOldPasswordVisible = false;
            _oldPasswordInputIcon = Icons.Material.Filled.Visibility;
            _oldPasswordInputType = InputType.Password;
        }
        else
        {
            _isOldPasswordVisible = true;
            _oldPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            _oldPasswordInputType = InputType.Text;
        }
    }

    private void ToggleNewPasswordVisibility()
    {
        if (_isNewPasswordVisible)
        {
            _isNewPasswordVisible = false;
            _newPasswordInputIcon = Icons.Material.Filled.Visibility;
            _newPasswordInputType = InputType.Password;
        }
        else
        {
            _isNewPasswordVisible = true;
            _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            _newPasswordInputType = InputType.Text;
        }
    }

    private void ToggleRepeatPasswordVisibility()
    {
        if (_isRepeatPasswordVisible)
        {
            _isRepeatPasswordVisible = false;
            _repeatPasswordInputIcon = Icons.Material.Filled.Visibility;
            _repeatPasswordInputType = InputType.Password;
        }
        else
        {
            _isRepeatPasswordVisible = true;
            _repeatPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            _repeatPasswordInputType = InputType.Text;
        }
    }

    private async Task OnDeleteAccountClicked()
    {
        var parameters = new DialogParameters<CustomDialog>
            {
                { x => x.Title, "Delete Account" },
                { x => x.Description, "Are you absolutely sure you want to delete your account? This action cannot be undone and all your data will be permanently deleted."},
                { x => x.MainButtonText, "Delete Account" },
                { x => x.MainButtonColor, Color.Error },
                { x => x.MainButtonVariant, Variant.Filled }
            };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };

        var dialog = await DialogService.ShowAsync<CustomDialog>(string.Empty, parameters, options);

        var result = await dialog.Result;

        bool isConfirmed = false;

        if (result.Data is not null)
        {
            isConfirmed = (bool)result.Data;
        }

        if (isConfirmed)
        {
            var response = await UsersService.DeleteAsync(UserId);

            if (!response.IsSuccess)
            {
                Snackbar.Add($"Error while deleting account: {response.ErrorMessage}", Severity.Error);
                return;
            }

            await AuthService.LogoutAsync();
            Navigation.NavigateTo("/login");
        }
    }
}
