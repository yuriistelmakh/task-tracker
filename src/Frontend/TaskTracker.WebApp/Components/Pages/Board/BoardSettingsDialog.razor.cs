using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.BoardMembers;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.Services.Auth;
using TaskTracker.WebApp.Components.Shared;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class BoardSettingsDialog
{
    [CascadingParameter]
    public IMudDialogInstance Dialog { get; private set; } = default!;

    [Parameter]
    public int BoardId { get; set; }

    [Inject]
    public IBoardsService BoardsService { get; private set; } = default!;

    [Inject]
    public IBoardMembersService BoardMembersService { get; private set; } = default!;

    [Inject]
    public ICurrentUserService CurrentUserService { get; private set; } = default!;

    [Inject]
    public IUsersService UsersService { get; private set; } = default!;

    [Inject]
    public UiStateService UiStateService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    private string? _title;

    private string? _description;

    private int _totalMembersCount => _members.Count;

    private List<MemberModel> _members = [];

    private int? _currentUserId;

    private BoardRole _currentUserRole;

    private int _userSearchPageSize = 5;

    private BoardRole _selectedInviteRole = BoardRole.Member;

    private string? _ownersCount;
    private string? _adminsCount;
    private string? _membersCount;
    private string? _visitorsCount;

    private readonly List<string> _backgroundColorOptions = [
        "#5A7863",
        "#90AB8B",
        "#3B4953",
        "#4E868E",
        "#6D8EA0",
        "#7986CB",
        "#9E8FB2",
        "#C27BA0",
        "#D0887A",
        "#D4A353",
        "#8D6E63",
        "#546E7A"
    ];

    private string _selectedColor = "#5A7863";

    private UserSummaryModel? _invitationUserSearch;

    protected override async Task OnInitializedAsync()
    {
        var boardResult = await BoardsService.GetAsync(BoardId);
        
        if (!boardResult.IsSuccess)
        {
            Snackbar.Add($"Error getting board: {boardResult.ErrorMessage}", Severity.Error);
            return;
        }

        var boardDto = boardResult.Value!;

        _title = boardDto.Title;
        _description = boardDto.Description;

        var boardMembersResult = await BoardMembersService.GetAllAsync(BoardId);

        if (!boardMembersResult.IsSuccess)
        {
            Snackbar.Add($"Error getting board members: {boardResult.ErrorMessage}", Severity.Error);
            return;
        }

        _members = boardMembersResult.Value!.Select(m => m.ToMemberModel()).ToList();

        _ownersCount = _members.Count(m => m.Role == BoardRole.Owner).ToString();
        _adminsCount = _members.Count(m => m.Role == BoardRole.Admin).ToString();
        _membersCount = _members.Count(m => m.Role == BoardRole.Member).ToString();
        _visitorsCount = _members.Count(m => m.Role == BoardRole.Visitor).ToString();

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

    //========================= GENERAL SETTINGS ==============================

    private async void SaveGeneralChanges()
    {
        var result = await BoardsService.UpdateAsync(BoardId, title: _title, description: _description);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error updating board's general settings: {result.ErrorMessage}");
            return;
        }

        Snackbar.Add("Board was successfully updated", Severity.Success);
        UiStateService.NotifyBoardSettingsChanged();
    }

    //========================= APPEARANCE SETTINGS ==============================

    private void OnColorClicked(string newColor)
    {
        _selectedColor = newColor;
    }

    private async Task SaveAppearanceChanges()
    {
        var result = await BoardsService.UpdateAsync(BoardId, color: _selectedColor);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error updating board's appearance settings: {result.ErrorMessage}");
            return;
        }

        Snackbar.Add("Board was successfully updated", Severity.Success);
        UiStateService.NotifyBoardSettingsChanged();
    }

    //========================= MEMBERS SETTINGS ==============================

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

        return result.Value!
            .Where(u => !existingMemberIds.Contains(u.Id))
            .Select(u => u.ToUserSummaryModel())
            .ToList();
    }

    private async Task SendInvitations()
    {
        if (_invitationUserSearch is null)
        {
            return;
        }

        var request = new SendInvitationRequest
        {
            InviterId = _currentUserId.Value,
            InviteeId = _invitationUserSearch.Id,
            Role = _selectedInviteRole
        };

        var result = await BoardMembersService.SendInvitationAsync(BoardId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
            return;
        }
        else
        {
            Snackbar.Add($"Invitation was sent", Severity.Success);
        }

        _invitationUserSearch = null;
        _selectedInviteRole = BoardRole.Member;
    }
}
