using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;

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
    public AuthenticationStateProvider AuthStateProvider { private get; set; } = default!;

    [Inject]
    public IAuthService AuthService { private get; set; } = default!;

    string search = "";

    string username = "";

    List<BoardModel> Boards = [];

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;

        if (!user.Identity!.IsAuthenticated)
        {
            return;
        }

        username = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var boardDtos = await BoardsService.GetAllAsync();

        if (boardDtos is null)
        {
            Snackbar.Add("Something went wrong", Severity.Error);
            return;
        }

        Boards = boardDtos.Select(bd => new BoardModel
        {
            Id = bd.Id,
            Title = bd.Title,
            IsArchived = bd.IsArchived,
            TasksCount = bd.TasksCount,
            MembersCount = bd.MembersCount,
            OwnerName = bd.Owner.DisplayName,
            OwnerIconUrl = bd.Owner.AvatarUrl
        }).ToList();
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
