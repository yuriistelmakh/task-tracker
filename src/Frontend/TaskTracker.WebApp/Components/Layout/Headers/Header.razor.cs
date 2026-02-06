using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using TaskTracker.Domain.DTOs.BoardMembers;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Components.Shared;
using TaskTracker.WebApp.Models.Mapping;
using TaskTracker.WebApp.Models.Notifications;

namespace TaskTracker.WebApp.Components.Layout.Headers;

public partial class Header
{
    [Parameter]
    public RenderFragment? LeftContent { get; set; }

    [Parameter]
    public RenderFragment? RightContent { get; set; }

    [Parameter]
    public RenderFragment? MiddleContent { get; set; }

    [Inject]
    public ICurrentUserService CurrentUserService { get; private set; } = default!;

    [Inject]
    public IUsersService UsersService { get; private set; } = default!;

    [Inject]
    public IBoardsService BoardsService { get; private set; } = default!;

    [Inject]
    public IAuthService AuthService { get; private set; } = default!;

    [Inject]
    public IBoardMembersService BoardMembersService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public NavigationManager Nav { get; private set; } = default!;

    [Inject]
    public BoardStateService BoardStateService { get; private set; } = default!;

    [Inject]
    public HeaderStateService HeaderStateService { get; private set; } = default!;

    private string _username = string.Empty;

    private string? _avatarUrl;

    private int? _currentUserId;

    private List<NotificationModel> _notifications { get; set; } = [];

    private bool _isNotificationsOpen = false;

    private bool _isProfileOpen = false;

    protected override async Task OnInitializedAsync()
    {
        HeaderStateService.OnUserUpdated += HandleUserUpdated;

        _currentUserId = await CurrentUserService.GetUserId();

        if (_currentUserId is not null)
        {
            var userResult = await UsersService.GetByIdAsync(_currentUserId.Value);
            if (userResult.IsSuccess && userResult.Value is not null)
            {
                _avatarUrl = userResult.Value.AvatarUrl;
                _username = userResult.Value.DisplayName;
            }

            var result = await UsersService.GetUnreadNotifications(_currentUserId.Value);

            if (!result.IsSuccess)
            {
                Snackbar.Add($"Error getting notifications: {result.ErrorMessage}", Severity.Error);
                return;
            }

            _notifications = result.Value!.Select(n => n.ToNotificationModel()).ToList();
            await InvokeAsync(StateHasChanged);
        }
    }

    private async void OnInvitationAcceptClicked(NotificationModel notification)
    {
        var invitation = JsonSerializer.Deserialize<InvitationPayload>(notification.Payload);

        if (invitation is null)
        {
            Snackbar.Add("Error deserializing invitation", Severity.Error);
            return;
        }

        var request = new AcceptInvitationRequest
        {
            InvitationId = invitation.Id,
            InviteeId = invitation.InviteeId,
            Role = invitation.Role,
            NotificationId = notification.Id
        };

        var result = await BoardMembersService.AcceptInvitationAsync(invitation.BoardId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error accepting an invitation: {result.ErrorMessage}", Severity.Error);
            return;
        }

        _notifications.Remove(notification);
        BoardStateService.NotifyBoardListChanged();

        if (_notifications.Count == 0)
        {
            _isNotificationsOpen = false;
        }

        StateHasChanged();
    }

    private async Task OnInivtationRejectClicked(NotificationModel notification)
    {
        var invitation = JsonSerializer.Deserialize<InvitationPayload>(notification.Payload);

        if (invitation is null)
        {
            Snackbar.Add("Error deserializing invitation", Severity.Error);
            return;
        }

        var request = new RejectInvitationRequest
        {
            InvitationId = invitation.Id,
            NotificationId = notification.Id
        };

        var result = await BoardMembersService.RejectInvitationAsync(invitation.BoardId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error rejecting an invitation: {result.ErrorMessage}", Severity.Error);
            return;
        }

        _notifications.Remove(notification);
        BoardStateService.NotifyBoardListChanged();

        if (_notifications.Count == 0)
        {
            _isNotificationsOpen = false;
        }

        StateHasChanged();
    }

    private void OnNotificationsClicked()
    {
        _isNotificationsOpen = true;
    }

    private void OnProfilePopoverClicked()
    {
        _isProfileOpen = true;
    }

    private void OnProfileClicked()
    {
        if (_currentUserId.HasValue)
        {
            Nav.NavigateTo($"/profile/{_currentUserId.Value}");
            _isProfileOpen = false;
        }
    }

    private async Task OnLogoutClicked()
    {
        Nav.NavigateTo("/login");
        await AuthService.LogoutAsync();
    }

    private async void HandleUserUpdated()
    {
        await OnInitializedAsync();
    }
}
