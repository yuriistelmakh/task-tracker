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
    public IBoardMembersService BoardMembersService {  get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public NavigationManager Nav { get; private set; } = default!;

    [Inject]
    public UiStateService UiStateService { get; private set; } = default!;

    private string username = string.Empty;

    private string search = string.Empty;

    private List<NotificationModel> _notifications { get; set; } = [];

    private bool _isNotificationsOpen = false;

    private bool _isProfileOpen = false;

    protected override async Task OnInitializedAsync()
    {
        username = await CurrentUserService.GetUserDisplayName() ?? "Anonymous";

        var userId = await CurrentUserService.GetUserId();

        if (userId is not null)
        {
            var result = await UsersService.GetUnreadNotifications(userId.Value);

            if (!result.IsSuccess)
            {
                Snackbar.Add($"Error getting notifications: {result.ErrorMessage}", Severity.Error);
                return;
            }

            _notifications = result.Value!.Select(n => n.ToNotificationModel()).ToList();
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
        UiStateService.NotifyBoardListChanged();

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
        UiStateService.NotifyBoardListChanged();

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

    private void OnProfileClicked()
    {
        _isProfileOpen = true;
    }

    private async Task OnLogoutClicked()
    {
        Nav.NavigateTo("/login");
        await AuthService.LogoutAsync();
    }
}
