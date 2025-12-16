using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;

namespace TaskTracker.WebApp.Components.Pages;

[Authorize]
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

    string search = "";

    string username = "";

    List<BoardVm> Boards = [];

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        username = user.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";

        var boardDtos = await BoardsService.GetAllAsync();

        if (boardDtos is null)
        {
            Snackbar.Add("Something went wrong", Severity.Error);
            return;
        }

        Boards = boardDtos.Select(bd => new BoardVm
        {
            Title = bd.Title,
            IsArchived = bd.IsArchived,
            TasksCount = bd.TasksCount,
            MembersCount = bd.MembersCount,
            OwnerName = bd.Owner.DisplayName,
            OwnerIconUrl = bd.Owner.AvatarUrl
        }).ToList();

    }
}
