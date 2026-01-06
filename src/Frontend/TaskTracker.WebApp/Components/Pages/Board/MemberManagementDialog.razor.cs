using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Cryptography.Xml;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Enums;
using TaskTracker.Services;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Components.Shared;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class MemberManagementDialog
{
    [Parameter]
    public int BoardId { get; set; }

    [CascadingParameter]
    public IMudDialogInstance Dialog { get; private set; } = default!;

    [Inject]
    public IBoardsService BoardsService { get; private set; } = default!;

    [Inject]
    public IUsersService UsersService { get; private set; } = default!;

    [Inject]
    public IBoardMembersService BoardMembersService {  get; private set; } = default!;

    [Inject]
    public ICurrentUserService CurrentUserService { get; private set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    private int _membersCount => _members.Count;

    private int? _currentUserId;

    private BoardRole _currentUserRole;

    private List<MemberModel> _members = [];
    
    private List<UserSummaryModel> _usersToInvite = [];

    private UserSummaryModel _invitationUserSearch;

    private BoardRole _inviteRole = BoardRole.Member;

    private int _userSearchPageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        var result = await BoardMembersService.GetAllAsync(BoardId);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error fetching members: {result.ErrorMessage}");
            return;
        }

        _members = result.Value!.Select(m => m.ToMemberModel()).ToList();

        _currentUserId = await CurrentUserService.GetUserId();
        var currentUser = _members.FirstOrDefault(m => m.Id == _currentUserId);

        if (currentUser is null)
        {
            Snackbar.Add("Error identifying current user", Severity.Error);
            return;
        }

        _currentUserRole = currentUser.Role;
    }

    private void OnCloseClicked()
    {
        Dialog.Close();
    }

    private void OnUserSelected(UserSummaryModel user)
    {
        if (user is not null && !_usersToInvite.Any(u => u.Id == user.Id))
        {
            _usersToInvite.Add(user);
        }
        
        _invitationUserSearch = null;

        StateHasChanged();
    }

    private void RemoveUserFromInvite(UserSummaryModel user)
    {
        _usersToInvite.Remove(user);
    }

    private async Task SendInvitations()
    {
        if (_usersToInvite.Count == 0)
        {
            return;
        }

        var request = new SendInvitationsRequest
        {
            InviterId = _currentUserId.Value,
            InviteeIds = _usersToInvite.Select(u => u.Id).ToList(),
            Role = _inviteRole
        };

        var result = await BoardMembersService.SendInvitationsAsync(BoardId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
            return;
        }
        else
        {
            Snackbar.Add($"Invitations were sent", Severity.Success);
        }

        _usersToInvite.Clear();
        _inviteRole = BoardRole.Member;
    }

    private async Task OnRoleChange(MemberModel member, BoardRole newRole)
    {
        if (newRole == BoardRole.Owner && member.Id != _currentUserId)
        {
            var parameters = new DialogParameters<CustomDialog>
            {
                { x => x.Title, "Warning" },
                { x => x.Description, @"Are you sure you want to make this user an owner of this board?
                                        You are not going to be able to change this user's role anymore."},
                { x => x.MainButtonText, "Change" },
                { x => x.MainButtonColor, Color.Error },
                { x => x.MainButtonVariant, Variant.Filled }
            };

            var dialog = await DialogService.ShowAsync<CustomDialog>(string.Empty, parameters);

            var dialogResult = await dialog.Result;

            if (dialogResult.Data is null || !(bool)dialogResult.Data)
            {
                return;
            }
        }

        if (member.Role == BoardRole.Owner && newRole != BoardRole.Owner && _members.Count(m => m.Role == BoardRole.Owner) <= 1)
        {
            Snackbar.Add("Board must have at least one owner", Severity.Warning);
            return;
        }

        if (_currentUserId == member.Id && newRole < member.Role)
        {
            var parameters = new DialogParameters<CustomDialog>
            {
                { x => x.Title, "Warning" },
                { x => x.Description, @"Are you sure you want to downgrade your role?"},
                { x => x.MainButtonText, "Change" },
                { x => x.MainButtonColor, Color.Error },
                { x => x.MainButtonVariant, Variant.Filled }
            };

            var dialog = await DialogService.ShowAsync<CustomDialog>(string.Empty, parameters);

            var dialogResult = await dialog.Result;

            if (dialogResult.Data is null || !(bool)dialogResult.Data)
            {
                return;
            }
        }

        var request = new UpdateBoardMemberRoleRequest
        {
            NewRole = newRole
        };

        var result = await BoardMembersService.UpdateRoleAsync(BoardId, member.Id, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error occurred changing permissions: {result.ErrorMessage}", Severity.Error);
            return;
        }

        if (_currentUserId == member.Id && newRole < member.Role)
        {
            member.Role = newRole;
            Dialog.Close();
            return;
        }

        member.Role = newRole;

        StateHasChanged();
    }

    private async Task<IEnumerable<UserSummaryModel>> PerformSearch(string prompt, CancellationToken token)
    {
        var result = await UsersService.SearchAsync(prompt ?? string.Empty, _userSearchPageSize);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error getting members: {result.ErrorMessage}", Severity.Error);
            return [];
        }

        var existingMemberIds = _members.Select(m => m.Id).ToHashSet();
        var selectedInviteIds = _usersToInvite.Select(u => u.Id).ToHashSet();

        return result.Value!
            .Where(u => !existingMemberIds.Contains(u.Id) && !selectedInviteIds.Contains(u.Id))
            .Select(u => u.ToUserSummaryModel())
            .ToList();
    }
}