using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Components.Shared;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages.Home;

public partial class Home
{
    [Inject]
    public IBoardsService BoardsService { private get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { private get; set; } = default!;

    [Inject]
    public NavigationManager Nav { private get; set; } = default!;

    [Inject]
    public ICurrentUserService UserService { private get; set; } = default!;

    [Inject]
    public IAuthService AuthService { private get; set; } = default!;

    [Inject]
    public IDialogService DialogService { private get; set; } = default!;

    [Inject]
    public UiStateService UiStateService { private get; set; } = default!;

    private List<BoardModel> _boards = [];

    private readonly int _pageSize = 4;

    private int _boardsPagesCount;

    private int _selectedBoardsPage = 1;

    protected override async Task OnInitializedAsync()
    {
        await FetchBoards();

        UiStateService.OnBoardListChanged += HandleBoardListChanged;
    }

    private async Task FetchBoards()
    {
        var result = await BoardsService.GetAllAsync(_selectedBoardsPage, _pageSize);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error while fetching boards: {result.ErrorMessage}", Severity.Error);
            return;
        }

        var boardDtos = result.Value!;

        _boards = boardDtos.Items.Select(bd => bd.ToBoardModel()).ToList();
        _boardsPagesCount = boardDtos.TotalCount / _pageSize + 1;
    }

    private async Task OnCreateBoardClicked()
    {
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true };

        var dialog = await DialogService.ShowAsync<CreateBoardDialog>(string.Empty, options);

        var dialogResult = await dialog.Result;

        if (dialogResult!.Data is not null)
        {
            await FetchBoards();
        }
    }

    private async void HandleBoardListChanged()
    {
        await FetchBoards();
        StateHasChanged();
    }

    private async Task OnPageChanged(int selectedPage)
    {
        _selectedBoardsPage = selectedPage;

        await FetchBoards();
        StateHasChanged();
    }

    private void OnBoardClicked(int boardId)
    {
        Nav.NavigateTo($"/boards/{boardId}");
    }
}
