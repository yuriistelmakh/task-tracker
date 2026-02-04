using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models.Mapping;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Users;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.WebApp.Components.Pages.Profile;

public partial class UserProfile
{
    [Parameter]
    public int UserId { get; set; }

    [Inject]
    public IUsersService UsersService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    private UserDetailsModel _user = new() { Email = string.Empty, DisplayName = string.Empty, Tag = string.Empty };

    private ProfileUpdateModel _profileUpdate = new();

    private bool _isEditingMode = false;

    private Variant _inputVariant => _isEditingMode ? Variant.Outlined : Variant.Text;

    private int _activeTab = 1;

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

    private async Task OnValidSubmit()
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
}
