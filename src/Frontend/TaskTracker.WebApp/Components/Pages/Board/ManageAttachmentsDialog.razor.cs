using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.WebApp.Components.Shared;
using TaskTracker.WebApp.Models;
using TaskTracker.WebApp.Models.Mapping;

namespace TaskTracker.WebApp.Components.Pages.Board;

public partial class ManageAttachmentsDialog
{
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; private set; } = default!;

    [Inject]
    public IJSRuntime JS { get; private set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; private set; } = default!;

    [Inject]
    public IAttachmentsService AttachmentsService { get; private set; } = default!;

    [Inject]
    public ICurrentUserService CurrentUserService { get; private set; } = default!;

    [Inject]
    public IDialogService DialogService { get; private set; } = default!;

    [Parameter]
    public List<AttachmentModel> Attachments { get; set; } = [];

    [Parameter]
    public int BoardId { get; set; }

    [Parameter]
    public int TaskId { get; set; }

    private MudFileUpload<IReadOnlyList<IBrowserFile>> _fileUpload = default!;
    private List<AttachmentModel> _attachments = [];
    private double _totalSizeKB;
    private bool _isDragging;
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    protected override void OnInitialized()
    {
        _attachments = new List<AttachmentModel>(Attachments);
        CalculateTotalSize();
    }

    private void CalculateTotalSize()
    {
        _totalSizeKB = _attachments.Sum(a => a.SizeKB);
    }

    private async Task OnInputFileChanged(InputFileChangeEventArgs e)
    {
        _isDragging = false;

        var currentUserId = await CurrentUserService.GetUserId();

        if (currentUserId is null)
        {
            Snackbar.Add("User not authenticated", Severity.Error);
            return;
        }

        foreach (var file in e.GetMultipleFiles(10))
        {
            if (file.Size > MaxFileSizeBytes)
            {
                Snackbar.Add($"File {file.Name} exceeds 10 MB limit", Severity.Warning);
                continue;
            }

            try
            {
                using var stream = file.OpenReadStream(MaxFileSizeBytes);

                var uploadResult = await AttachmentsService.CreateAsync(
                    BoardId,
                    TaskId,
                    currentUserId.Value,
                    stream,
                    file.Name,
                    file.ContentType
                );

                if (!uploadResult.IsSuccess)
                {
                    Snackbar.Add($"Failed to upload {file.Name}: {uploadResult.ErrorMessage}", Severity.Error);
                    continue;
                }
                
                var getResult = await AttachmentsService.GetAllAsync(BoardId, TaskId);

                if (!getResult.IsSuccess)
                {
                    Snackbar.Add($"Failed to get attachments: {getResult.ErrorMessage}", Severity.Error);
                }

                _attachments = getResult.Value!.Select(a => a.ToAttachmentModel()).ToList();

                Snackbar.Add($"Uploaded {file.Name}", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error uploading {file.Name}: {ex.Message}", Severity.Error);
            }
        }

        CalculateTotalSize();
        StateHasChanged();
    }

    private static string GetAttachmentIcon(FileType fileType) =>
        fileType switch
        {
            FileType.Document => Icons.Material.Filled.Description,
            FileType.Spreadsheet => Icons.Material.Filled.TableChart,
            FileType.Audio => Icons.Material.Filled.Audiotrack,
            FileType.Video => Icons.Material.Filled.Videocam,
            FileType.Image => Icons.Material.Filled.Image,
            _ => Icons.Material.Filled.Attachment,
        };

    private async Task OnAttachmentClicked(AttachmentModel attachment)
    {
        try
        {
            await JS.InvokeVoidAsync("openFile", attachment.FileUrl, attachment.Name);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error opening file: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnDeleteAttachmentAsync(AttachmentModel attachment)
    {
        var parameters = new DialogParameters<CustomDialog>
        {
            { x => x.Title, "Delete Attachment" },
            { x => x.Description, $"Are you sure you want to delete '{attachment.Name}'? This action cannot be undone." },
            { x => x.MainButtonText, "Delete" },
            { x => x.MainButtonColor, Color.Error },
            { x => x.MainButtonVariant, Variant.Filled }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CustomDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;

        if (result.Data is not bool confirmed || !confirmed)
        {
            return;
        }

        var deleteResult = await AttachmentsService.DeleteAsync(BoardId, TaskId, attachment.Id);

        if (!deleteResult.IsSuccess)
        {
            Snackbar.Add($"Error deleting attachment: {deleteResult.ErrorMessage}", Severity.Error);
            return;
        }

        _attachments.Remove(attachment);
        CalculateTotalSize();
        Snackbar.Add($"Deleted {attachment.Name}", Severity.Success);
    }

    private async Task OnRenameAttachmentAsync(AttachmentModel attachment)
    {
        var parameters = new DialogParameters<RenameDialog>
        {
            { x => x.Title, "Rename Attachment" },
            { x => x.CurrentName, attachment.Name }
        };

        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<RenameDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;

        if (result.Canceled || result.Data is not string newName || string.IsNullOrWhiteSpace(newName))
        {
            return;
        }

        var renameResult = await AttachmentsService.RenameAsync(BoardId, TaskId, attachment.Id, newName);

        if (!renameResult.IsSuccess)
        {
            Snackbar.Add($"Error renaming attachment: {renameResult.ErrorMessage}", Severity.Error);
            return;
        }

        attachment.Name = newName;
        Snackbar.Add($"Renamed to {newName}", Severity.Success);
        StateHasChanged();
    }

    private void OnCloseClicked()
    {
        MudDialog.Cancel();
    }

    private void OnSaveClicked()
    {
        MudDialog.Close(DialogResult.Ok(_attachments));
    }
}
