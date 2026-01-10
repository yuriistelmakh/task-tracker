using Microsoft.AspNetCore.Components;
using MudBlazor;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Models.Mapping;
using TaskTracker.WebApp.Models;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Enums;
using TaskTracker.WebApp.Components.Shared;
using Microsoft.AspNetCore.Components.Web;
using TaskTracker.WebApp.Models.Tasks;
using TaskTracker.Services.Auth;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class TaskDialog
{
    [Parameter]
    public int TaskId { get; set; }

    [Parameter]
    public int BoardId { get; set; }

    [Parameter]
    public BoardRole CurrentUserRole { get; set; }

    [CascadingParameter]
    IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject]
    public ICurrentUserService UserService { get; private set; } = default!;

    [Inject]
    public IBoardsService BoardsService { get; private set; } = default!;

    [Inject]
    public ITasksService TasksService { get; private set; } = default!;

    [Inject]
    public IBoardMembersService BoardMembersService {  get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    private TaskDetailsModel task = new() { ColumnTitle = string.Empty, Title = string.Empty };

    private bool _isTaskLoaded = false;

    private bool _isTitleEditing = false;

    private bool _isDescriptionExpanded = false;

    private string _titleBeforeEditing = string.Empty;

    private int _pageSize = 4;

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
        var result = await TasksService.GetByIdAsync(BoardId, TaskId);
        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error while fetching data about task: {result.ErrorMessage}", Severity.Error);
            return;
        }

        var dto = result.Value!;
        task = dto.ToTaskDetailsModel();
        
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

        var result = await TasksService.UpdateAsync(BoardId, TaskId, request);

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
            var response = await TasksService.DeleteAsync(BoardId, task.Id);

            if (!response.IsSuccess)
            {
                Snackbar.Add($"Error while deleting task: {response.ErrorMessage}", Severity.Error);
                return;
            }

            MudDialog.Close(DialogResult.Ok(new TaskDialogResult(TaskDialogAction.Delete)));
        }
    }

    private async Task<IEnumerable<MemberModel>> PerformSearch(string prompt, CancellationToken token)
    {
        var result = await BoardMembersService.SearchAsync(BoardId, prompt, 1, _pageSize);

        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error getting members: {result.ErrorMessage}", Severity.Error);
            return [];
        }

        var members = result.Value!.Select(m => m.ToMemberModel()).ToList();

        return members;
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

    private void OnExtendDescription()
    {
        _isDescriptionExpanded = !_isDescriptionExpanded;
    }

    private void OnCloseClicked()
    {
        MudDialog.Close();
    }
}
