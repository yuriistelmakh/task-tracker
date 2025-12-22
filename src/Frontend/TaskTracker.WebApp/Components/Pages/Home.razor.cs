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

    string search = "";

    string username = "";

    List<BoardModel> Boards = [];

    protected override async Task OnInitializedAsync()
    {
        username = await UserService.GetUserDisplayName() ?? "Anonymous";

        var boardDtos = await BoardsService.GetAllAsync();

        if (boardDtos is null)
        {
            Snackbar.Add("Something went wrong", Severity.Error);
            return;
        }

        Boards = boardDtos.Select(bd => bd.ToBoardModel()).ToList();
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
