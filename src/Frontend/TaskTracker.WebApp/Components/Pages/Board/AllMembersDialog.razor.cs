using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Services;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class AllMembersDialog
{
    [Parameter]
    public int BoardId { get; set; }

    [Inject]
    public IBoardMembersService BoardMembersService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [CascadingParameter]
    public IMudDialogInstance Dialog { get; private set; } = default!;

    private List<MemberModel> _members = [];

    private int _totalMembersCount;

    private int _totalMembersPages => (int)Math.Ceiling((double)_totalMembersCount / _pageSize);

    private int _currentMembersPage = 1;

    private int _pageSize = 4;

    private string? _searchPrompt;

    protected override async Task OnInitializedAsync()
    {
        await FetchMembers(string.Empty);
    }

    private async Task OnPageChanged(int page)
    {
        _currentMembersPage = page;
        await FetchMembers(_searchPrompt);
    }

    private async Task FetchMembers(string? prompt)
    {
        _searchPrompt = prompt;
        var result = await BoardMembersService.SearchAsync(BoardId, _searchPrompt, _currentMembersPage, _pageSize);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error getting members: {result.ErrorMessage}");
            return;
        }

        _members = result.Value!.Items.Select(m => m.ToMemberModel()).ToList();
        _totalMembersCount = result.Value!.TotalCount;

        StateHasChanged();
    }

    private void OnCloseClicked()
    {
        Dialog.Close();
    }
}
