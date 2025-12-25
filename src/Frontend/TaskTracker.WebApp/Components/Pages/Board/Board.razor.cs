using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Security.Cryptography.X509Certificates;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class Board
{
    [Parameter]
    public int BoardId { get; set; }

    [Inject]
    public IBoardsService BoardsService { get; set; } = default!;

    [Inject]
    public IColumnsService ColumnsService { get; set; } = default!;

    [Inject]
    public ITasksService TasksService { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private MudDropContainer<TaskSummaryModel> _dropContainer = default!;

    private bool _isAddColumnOpen = false;

    private string _addColumnTitle = string.Empty;

    private List<TaskSummaryModel> _allTasks = [];

    private List<ColumnModel> _columns = [];

    protected override async Task OnInitializedAsync()
    {
        var result = await BoardsService.GetAsync(BoardId);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error while fetching the board: {result.ErrorMessage}", Severity.Error);
            return;
        }

        var boardDto = result.Value!;

        _columns = boardDto.Columns.Select(c => c.ToColumModel())
            .OrderBy(c => c.Order)
            .ToList();

        _allTasks = boardDto.Columns
            .SelectMany(c => c.Tasks.Select(t => t.ToTaskSummaryModel()))
            .OrderBy(t => t.ColumnId)
            .ThenBy(t => t.Order)
            .ToList();
    }

    private static void OnAddTaskClick(ColumnModel column)
    {
        column.IsAddTaskOpen = true;
    }

    private async Task HandleAddTaskEnter(KeyboardEventArgs e, ColumnModel column)
    {
        if (e.Key == "Enter" && !e.ShiftKey)
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

        var newTask = new TaskSummaryModel
        {
            Title = titleToSend,
            Priority = Priority.Medium,
            Order = column.Tasks.Count,
            ColumnId = column.Id
        };

        column.NewTaskTitle = string.Empty;
        column.IsAddTaskOpen = false;

        column.Tasks.Add(newTask);

        _allTasks.Add(newTask);

        _dropContainer.Refresh();

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
            _allTasks.Remove(newTask);
            _dropContainer.Refresh();
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

    private async void OnCheckedChange(TaskSummaryModel task)
    {
        task.IsComplete = !task.IsComplete;

        var request = new ChangeTaskStatusRequest
        {
            IsComplete = task.IsComplete
        };

        _dropContainer.Refresh();

        var result = await TasksService.ChangeStatusAsync(task.Id, request);

        if (!result.IsSuccess)
        {
            task.IsComplete = !task.IsComplete;

            Snackbar.Add($"Error occurred: {result.ErrorMessage}", Severity.Error);

            _dropContainer.Refresh();
        }
    }

    private async Task HandleAddColumnEnter(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await AddNewColumn();
        }
    }

    private void OnAddColumnClick()
    {
        _isAddColumnOpen = true;
    }

    private void AddNewColumnClose()
    {
        _isAddColumnOpen = false;
        _addColumnTitle = string.Empty;
    }

    private async Task AddNewColumn()
    {
        if (string.IsNullOrEmpty(_addColumnTitle))
        {
            return;
        }

        var titleToSend = _addColumnTitle;

        var newColumn = new ColumnModel
        {
            Order = _columns.Count,
            Title = _addColumnTitle
        };

        _addColumnTitle = string.Empty;
        _isAddColumnOpen = false;

        _columns.Add(newColumn);

        var request = new CreateColumnRequest
        {
            Title = titleToSend,
            Order = newColumn.Order,
            BoardId = BoardId,
        };

        var result = await ColumnsService.CreateAsync(request);

        if (!result.IsSuccess)
        {
            _columns.Remove(newColumn);
            _isAddColumnOpen = true;
            _addColumnTitle = titleToSend;

            Snackbar.Add($"Error occurred: {result.ErrorMessage}", Severity.Error);
            return;
        }

        newColumn.Id = result.Value;
    }

    private static string GetColorForPriority(Priority priority) => priority switch
    {
        Priority.High => "var(--mud-palette-error)",
        Priority.Medium => "var(--mud-palette-warning)",
        Priority.Low => "var(--mud-palette-success-darken)",
        _ => "var(--mud-palette-divider)"
    };

    private async Task TaskDropped(MudItemDropInfo<TaskSummaryModel> dropItem)
    {
        var originalColumnId = dropItem.Item.ColumnId;
        var originalOrder = dropItem.Item.Order;
        var newColumnId = int.Parse(dropItem.DropzoneIdentifier);

        dropItem.Item.ColumnId = newColumnId;

        var tasksInTargetColumn = _allTasks
            .Where(x => x.ColumnId == newColumnId && x.Id != dropItem.Item.Id)
            .OrderBy(x => x.Order)
            .ToList();

        var newIndex = Math.Clamp(dropItem.IndexInZone, 0, tasksInTargetColumn.Count);
        tasksInTargetColumn.Insert(newIndex, dropItem.Item);

        for (int i = 0; i < tasksInTargetColumn.Count; i++)
        {
            tasksInTargetColumn[i].Order = i;
        }

        var request = new ReorderColumnTasksRequest
        {
            MovedTaskId = dropItem.Item.Id,
            ColumnId = newColumnId,
            MoveTaskRequests = tasksInTargetColumn.Select(t => new MoveTaskRequest
            {
                TaskId = t.Id,
                NewOrder = t.Order
            }).ToList()
        };

        if (originalColumnId != newColumnId)
        {
            var tasksInSourceColumn = _allTasks
                .Where(x => x.ColumnId == originalColumnId)
                .OrderBy(x => x.Order)
                .ToList();

            for (int i = 0; i < tasksInSourceColumn.Count; i++)
            {
                tasksInSourceColumn[i].Order = i;
            }

            request.MoveTaskRequests = request.MoveTaskRequests
                .Concat(tasksInSourceColumn.Select(t => new MoveTaskRequest
                {
                    TaskId = t.Id,
                    NewOrder = t.Order
                }))
                .ToList();
        }

        _allTasks = _allTasks
            .OrderBy(t => t.ColumnId)
            .ThenBy(t => t.Order)
            .ToList();

        var result = await ColumnsService.ReorderAsync(newColumnId, request);

        if (!result.IsSuccess)
        {
            dropItem.Item.ColumnId = originalColumnId;
            dropItem.Item.Order = originalOrder;

            _allTasks = _allTasks.OrderBy(t => t.ColumnId).ThenBy(t => t.Order).ToList();

            _dropContainer.Refresh();
            Snackbar.Add($"Error moving task: {result.ErrorMessage}", Severity.Error);
        }
    }

    private async Task OnTaskClicked(int taskId)
    {
        var parameters = new DialogParameters<TaskDialog>
        {
            { x => x.TaskId, taskId },
            { x => x.BoardId, BoardId }
        };

        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true, };

        var dialog = await DialogService.ShowAsync<TaskDialog>(string.Empty, parameters, options);

        var result = await dialog.Result;

        if (result is not null && result.Data is not null)
        {
            var dialogResult = (TaskDialogResult)result.Data;

            if (dialogResult.Action == TaskDialogAction.Delete)
            {
                var taskToRemove = _allTasks.FirstOrDefault(t => t.Id == taskId);
                if (taskToRemove != null)
                {
                    _allTasks.Remove(taskToRemove);
                    _dropContainer.Refresh();
                }
            }
            else if (dialogResult.Action == TaskDialogAction.Update && dialogResult.Task is not null)
            {
                var updatedTask = dialogResult.Task;
                var taskInList = _allTasks.FirstOrDefault(t => t.Id == updatedTask.Id);

                if (taskInList is not null)
                {
                    taskInList.Title = updatedTask.Title;
                    taskInList.Priority = updatedTask.Priority;
                    _dropContainer.Refresh();
                }
            }
        }
    }
}
