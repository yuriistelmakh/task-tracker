using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;

namespace TaskTracker.WebApp.Components.Pages;

public partial class Board
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IBoardsService BoardsService { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    int columnsCount = 4;

    int spacing => 12 / columnsCount;

    List<ColumnVm> Columns = [];

    protected override async Task OnInitializedAsync()
    {
        var dto = await BoardsService.GetAsync(Id);
        
        if (dto is null)
        {
            Snackbar.Add("Board was not found.", Severity.Error);
            return;
        }

        Columns = dto.Columns.Select(c => new ColumnVm
        {
            Title = c.Title,
            Order = c.Order,
            Tasks = c.Tasks.Select(t => new TaskVm
            {
                Title = t.Title,
                Priority = t.Priority,
                Order = t.Order
            }).OrderBy(t => t.Order)
        })
        .OrderBy(c => c.Order)
        .ToList() ?? [];
    }

    private string GetColorForPriority(Priority priority) => priority switch
    {
        Priority.High => "var(--mud-palette-error)",
        Priority.Medium => "var(--mud-palette-warning)",
        Priority.Low => "var(--mud-palette-success-darken)",
        _ => "var(--mud-palette-divider)"
    };
}
