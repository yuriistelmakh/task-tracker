using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Enums;
using TaskTracker.Services;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages;

public partial class Board
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IBoardsService BoardsService { get; set; } = default!;

    [Inject]
    public ITasksService TasksService { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    int columnsCount = 4;

    int spacing => 12 / columnsCount;

    List<ColumnModel> Columns = [];

    protected override async Task OnInitializedAsync()
    {
        var dto = await BoardsService.GetAsync(Id);
        
        if (dto is null)
        {
            Snackbar.Add("Board was not found.", Severity.Error);
            return;
        }


        Columns = dto.Columns.Select(c => c.ToColumModel())
            .OrderBy(c => c.Order)
            .ToList();
    }

    private static void OnAddTaskClick(ColumnModel column)
    {
        column.IsAddTaskOpen = true;
    }

    private async Task HandleEnter(KeyboardEventArgs e, ColumnModel column)
    {
        if (e.Key == "Enter")
        {
            await AddNewTask(column);
        }
    }

    private async Task AddNewTask(ColumnModel column)
    {
        if (string.IsNullOrEmpty(column.NewTaskTitle))
        {
            return;
        }

        var titleToSend = column.NewTaskTitle;

        var newTask = new TaskModel
        {
            Title = titleToSend,
            Priority = Priority.Medium,
            Order = column.Tasks.Count
        };

        column.NewTaskTitle = string.Empty;
        column.IsAddTaskOpen = false;

        column.Tasks.Add(newTask);

        var request = new CreateTaskRequest
        {
            ColumnId = column.Id,
            Order = newTask.Order,
            Title = titleToSend
        };

        var result = await TasksService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            column.Tasks.Remove(newTask);
            column.IsAddTaskOpen = true;
            column.NewTaskTitle = titleToSend;

            Snackbar.Add($"Error occurred: {result.ErrorMessage}", Severity.Error);
            return;
        }

        newTask.Id = result.Value;
    }

    private static void AddTaskClose(ColumnModel column)
    {
        column.IsAddTaskOpen = false;
        column.NewTaskTitle = string.Empty;
    }
    
    private async void OnCheckedChange(TaskModel task)
    {
        task.IsComplete = !task.IsComplete;

        var request = new ChangeTaskStatusRequest
        {
            IsComplete = task.IsComplete
        };

        var result = await TasksService.ChangeStatusAsync(task.Id, request);

        if (!result.IsSuccess)
        {
            task.IsComplete = !task.IsComplete;

            Snackbar.Add($"Error occurred: {result.ErrorMessage}", Severity.Error);
        }
    }

    private string GetColorForPriority(Priority priority) => priority switch
    {
        Priority.High => "var(--mud-palette-error)",
        Priority.Medium => "var(--mud-palette-warning)",
        Priority.Low => "var(--mud-palette-success-darken)",
        _ => "var(--mud-palette-divider)"
    };
}
