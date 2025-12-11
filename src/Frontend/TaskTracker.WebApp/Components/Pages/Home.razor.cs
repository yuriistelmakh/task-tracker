using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.WebApp.Components.Pages;

public partial class Home
{
    [Inject]
    IBoardsService BoardsService { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }

    string _search = "";

    List<BoardVm> Boards = [];

    protected override async Task OnInitializedAsync()
    {
        var boardDtos = await BoardsService.GetAllAsync(1);

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

    public class BoardVm
    {
        public required string Title { get; set; }

        public bool IsArchived { get; set; }

        public int TasksCount { get; set; }

        public int MembersCount { get; set; }

        public required string OwnerName { get; set; }

        public string? OwnerIconUrl { get; set; }
    }

}
