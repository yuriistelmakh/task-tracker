using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages;

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

    private string _search = "";

    private string _username = "";

    private List<BoardModel> _boards = [];

    protected override async Task OnInitializedAsync()
    {
        if (!await UserService.IsUserAuthenticated())
        {
            return;
        }

        _username = await UserService.GetUserDisplayName() ?? "Anonymous";

        var result = await BoardsService.GetAllAsync();

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Eroor while fetching boards: {result.ErrorMessage}", Severity.Error);
            return;
        }

        var boardDtos = result.Value!;

        _boards = boardDtos.Select(bd => bd.ToBoardModel()).ToList();
    }

    private async Task Logout()
    {
        await AuthService.LogoutAsync();
        Nav.NavigateTo("/login");
    }

    private void OnBoardClicked(int boardId)
    {
        Nav.NavigateTo($"/boards/{boardId}");
    }
}
