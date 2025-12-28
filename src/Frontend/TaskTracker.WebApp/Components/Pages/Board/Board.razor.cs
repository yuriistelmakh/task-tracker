using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Components.Shared;
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

    [Inject]
    public IJSRuntime JS { get; set; } = default!;

    private MudDropContainer<TaskSummaryModel> _taskDropContainer = default!;

    private MudDropContainer<ColumnModel> _columnDropContainer = default!;

    private bool _isAddColumnOpen = false;

    private string _addColumnTitle = string.Empty;

    private bool _isReorderingColumns = false;

    private List<TaskSummaryModel> _allTasks = [];

    private List<ColumnModel> _columns = [];

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("registerPageAutoScroll");
        }
    }

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

        _taskDropContainer.Refresh();

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
            _taskDropContainer.Refresh();
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

        _taskDropContainer.Refresh();

        var result = await TasksService.ChangeStatusAsync(task.Id, request);

        if (!result.IsSuccess)
        {
            task.IsComplete = !task.IsComplete;

            Snackbar.Add($"Error occurred: {result.ErrorMessage}", Severity.Error);

            _taskDropContainer.Refresh();
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

            _taskDropContainer.Refresh();
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
                    _taskDropContainer.Refresh();
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
                    _taskDropContainer.Refresh();
                }
            }
        }
    }

    private void HandleEditColumnEnter(KeyboardEventArgs args, ColumnModel column)
    {
        if (args.Key == "Enter")
        {
            EditColumn(column);
            column.IsEditing = false;
        }
    }

    private async void EditColumn(ColumnModel column)
    {
        if (string.IsNullOrEmpty(column.Title))
        {
            column.Title = column.OldTitle!;
            return;
        }

        var request = new UpdateColumnRequest
        {
            Title = column.Title,
            Order = column.Order
        };

        var result = await ColumnsService.UpdateAsync(column.Id, request);

        if (!result.IsSuccess)
        {
            column.Title = column.OldTitle!;
            Snackbar.Add($"Error editing column: {result.ErrorMessage}");
        }

        column.IsEditing = false;
    }

    private void EditColumnClose(ColumnModel column)
    {
        column.Title = column.OldTitle!;
        column.IsEditing = false;
    }

    private void OnEditColumnClick(ColumnModel column)
    {
        column.IsEditing = true;
        column.OldTitle = column.Title;
    }

    private async Task OnDeleteColumnClick(ColumnModel column)
    {
        var parameters = new DialogParameters<CustomDialog>
        {
            { x => x.Title, "Warning" },
            { x => x.Description, "Are you sure you want to delete this column with all tasks inside?"},
            { x => x.MainButtonText, "Delete" },
            { x => x.MainButtonColor, Color.Error },
            { x => x.MainButtonVariant, Variant.Filled }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };

        var dialog = await DialogService.ShowAsync<CustomDialog>(string.Empty, parameters, options);

        var result = await dialog.Result;

        bool isConfirmed = false;

        if (result.Data is not null)
        {
            isConfirmed = (bool)result.Data;
        }
        
        if (isConfirmed == true)
        {
            var response = await ColumnsService.DeleteAsync(column.Id);

            if (!response.IsSuccess)
            {
                Snackbar.Add($"Error while deleting column: {response.ErrorMessage}", Severity.Error);
                return;
            }

            _columns.Remove(column);
        }
    }

    private void ToggleReorderMode()
    {
        _isReorderingColumns = !_isReorderingColumns;
    }

    private async Task ColumnDropped(MudItemDropInfo<ColumnModel> dropItem)
    {
        var newIndex = dropItem.IndexInZone;
        
        var column = dropItem.Item;
        _columns.Remove(column);
        
        newIndex = Math.Clamp(newIndex, 0, _columns.Count);
        _columns.Insert(newIndex, column);

        for (int i = 0; i < _columns.Count; i++)
        {
            _columns[i].Order = i;
        }

        _columnDropContainer.Refresh();

        var request = new ReorderBoardColumnsRequest
        {
            MoveColumnRequests = _columns.Select(c => new MoveColumnRequest
            {
                ColumnId = c.Id,
                NewOrder = c.Order
            }).ToList()
        };

        var result = await BoardsService.ReorderColumnsAsync(BoardId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error moving column: {result.ErrorMessage}", Severity.Error);
            
            await OnInitializedAsync();
            _columnDropContainer.Refresh();
        }
    }
}
