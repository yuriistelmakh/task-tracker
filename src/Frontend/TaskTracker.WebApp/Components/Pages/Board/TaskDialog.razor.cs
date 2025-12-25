using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models.Mapping;
using TaskTracker.WebApp.Models;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Enums;
using System.Security.Cryptography.X509Certificates;
using TaskTracker.WebApp.Components.Shared;
using System.Security.Cryptography.Xml;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Components.Web;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class TaskDialog
{
    [Parameter]
    public int TaskId { get; set; }

    [Parameter]
    public int BoardId { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject]
    public ITasksService TasksService { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public ICurrentUserService UserService { get; private set; } = default!;

    [Inject]
    public IBoardsService BoardsService { get; private set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    private List<UserSummaryModel> _assigneeOptions = [];

    private TaskDetailsModel task = new() { ColumnTitle = string.Empty, Title = string.Empty };

    private bool _isTaskLoaded = false;

    private bool _isTitleEditing = false;

    private string _titleBeforeEditing;

    private TimeSpan? TaskTime
    {
        get => task.DueDate?.TimeOfDay;
        set
        {
            if (task.DueDate.HasValue)
            {
                task.DueDate = task.DueDate.Value.Date + (value ?? TimeSpan.Zero);
            }
            else
            {
                task.DueDate = DateTime.Today + (value ?? TimeSpan.Zero);
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        var result = await TasksService.GetByIdAsync(TaskId);
        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error while fetching data about task: {result.ErrorMessage}", Severity.Error);
            return;
        }

        var dto = result.Value!;
        task = dto.ToTaskDetailsModel();

        if (dto.AssigneeDto is not null)
        {
            task.AssigneeModel = dto.AssigneeDto.ToUserSummaryModel();
            _assigneeOptions.Add(task.AssigneeModel);
        }

        if (_assigneeOptions.Count > 1)
        {
            return;
        }

        var boardResult = await BoardsService.GetMembersAsync(BoardId);
        if (!boardResult.IsSuccess)
        {
            Snackbar.Add($"Error fetching board members: {result.ErrorMessage}", Severity.Error);
            return;
        }

        _assigneeOptions = boardResult.Value!.Select(u => u.ToUserSummaryModel()).ToList();
        _isTaskLoaded = true;
    }

    private async Task SaveChanges()
    {
        if (task is null)
        {
            Snackbar.Add("Wait until window loads", Severity.Warning);
            return;
        }

        var request = new UpdateTaskRequest
        {
            AssigneeId = task.AssigneeModel?.Id,
            Description = task.Description,
            DueDate = task.DueDate,
            IsComplete = task.IsComplete,
            Priority = task.Priority,
            Title = task.Title,
        };

        var result = await TasksService.UpdateAsync(TaskId, request);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error updating task data: {result.ErrorMessage}", Severity.Error);
            return;
        }

        MudDialog.Close(DialogResult.Ok(new TaskDialogResult(TaskDialogAction.Update, task)));
    }

    private async Task OnDeleteClick()
    {
        var parameters = new DialogParameters<CustomDialog>
        {
            { x => x.Title, "Warning" },
            { x => x.Description, "Are you sure you want to delete this task?"},
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
            var response = await TasksService.DeleteAsync(task.Id);

            if (!response.IsSuccess)
            {
                Snackbar.Add($"Error while deleting task: {response.ErrorMessage}", Severity.Error);
                return;
            }

            MudDialog.Close(DialogResult.Ok(new TaskDialogResult(TaskDialogAction.Delete)));
        }
    }

    private void EnableEditTitle()
    {
        _titleBeforeEditing = task.Title;
        _isTitleEditing = true;
    }

    private void SaveTitle()
    {
        if (string.IsNullOrEmpty(task.Title))
        {
            task.Title = _titleBeforeEditing;
        }

        _titleBeforeEditing = task.Title;
        _isTitleEditing = false;
    }

    private async Task HandleTitleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            SaveTitle();
        }
    }

    private void OnCloseClick()
    {
        MudDialog.Close();
    }
}
