using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Domain.DTOs.BoardMembers;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
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
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public NavigationManager Nav { get; private set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    private MudForm _titleForm;

    private string? _title;

    private string? _description;

    private int _pageSize = 4;

    private int _totalMembersCount;
    private int _totalMembersPages => _totalMembersCount / _pageSize + 1;

    private int _currentMembersPage = 1;

    private List<MemberModel> _members = [];

    private int? _currentUserId;

    private BoardRole _currentUserRole;

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
        _totalMembersCount = boardDto.MemberStatistics.TotalMembers;

        _ownersCount = boardDto.MemberStatistics.OwnersCount.ToString();
        _adminsCount = boardDto.MemberStatistics.AdministratorsCount.ToString();
        _membersCount = boardDto.MemberStatistics.MembersCount.ToString();
        _visitorsCount = boardDto.MemberStatistics.VisitorsCount.ToString();

        var boardMembersResult = await BoardMembersService.GetAllAsync(BoardId, _currentMembersPage, _pageSize);

        if (!boardMembersResult.IsSuccess)
        {
            Snackbar.Add($"Error getting board members: {boardMembersResult.ErrorMessage}", Severity.Error);
            return;
        }

        _members = boardMembersResult.Value!.Select(m => m.ToMemberModel()).ToList();

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
        await _titleForm.Validate();

        if (!_titleForm.IsValid)
        {
            return;
        }

        var result = await BoardsService.UpdateAsync(BoardId, title: _title, description: _description);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error updating board's general settings: {result.ErrorMessage}");
            return;
        }

        Snackbar.Add("Board was successfully updated", Severity.Success);
        UiStateService.NotifyBoardSettingsChanged();
    }

    private async Task OnDeleteBoardClicked()
    {
        var parameters = new DialogParameters<CustomDialog>
        {
            { x => x.Title, "Warning" },
            { x => x.Description, @"Are you sure you want to delete this board with it's tasks? This is permanent."},
            { x => x.MainButtonText, "Delete" },
            { x => x.MainButtonColor, Color.Error },
            { x => x.MainButtonVariant, Variant.Filled }
        };

        var dialog = await DialogService.ShowAsync<CustomDialog>(string.Empty, parameters);

        var dialogResult = await dialog.Result;

        if (dialogResult.Data is null || !(bool)dialogResult.Data)
        {
            return;
        }

        var result = await BoardsService.DeleteAsync(BoardId);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error deleting board: {result.ErrorMessage}", Severity.Error);
            return;
        }

        Nav.NavigateTo("/");
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

    private async Task<IEnumerable<UserSummaryModel>> PerformAutocompleteSearch(string prompt, CancellationToken token)
    {
        var result = await UsersService.SearchAsync(prompt ?? string.Empty, _pageSize);

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

    private async Task PerformGlobalSearch(string prompt)
    {
        var result = await BoardMembersService.SearchAsync(BoardId, prompt, _currentMembersPage, _pageSize);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error getting members: {result.ErrorMessage}");
            return;
        }

        _members = result.Value!.Items.Select(m => m.ToMemberModel()).ToList();
        _totalMembersCount = result.Value!.TotalCount;

        StateHasChanged();
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

    private async Task OnPageChanged(int page)
    {
        _currentMembersPage = page;

        var boardMembersResult = await BoardMembersService.GetAllAsync(BoardId, _currentMembersPage, _pageSize);

        if (!boardMembersResult.IsSuccess)
        {
            Snackbar.Add($"Error getting board members: {boardMembersResult.ErrorMessage}", Severity.Error);
            return;
        }

        _members = boardMembersResult.Value!.Select(m => m.ToMemberModel()).ToList();

        StateHasChanged();
    }
}
