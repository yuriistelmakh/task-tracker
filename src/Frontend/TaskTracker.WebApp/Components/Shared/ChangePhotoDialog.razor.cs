using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.WebApp.Components.Shared;

public partial class ChangePhotoDialog
{
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    public IUsersService UsersService { get; set; } = default!;

    [Parameter]
    public int UserId { get; set; }

    private IBrowserFile? _selectedFile;
    private string? _selectedFileName;
    private string? _previewUrl;
    private bool _isUploading;

    private const long MaxFileSize = 5 * 1024 * 1024;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        _selectedFile = null;
        _selectedFileName = null;
        _previewUrl = null;

        var file = e.File;
        if (file is null)
        {
            StateHasChanged();
            return;
        }

        var extension = Path.GetExtension(file.Name).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
        {
            Snackbar.Add($"Invalid file type. Please select an image file ({string.Join(", ", _allowedExtensions)})", Severity.Error);
            StateHasChanged();
            return;
        }

        if (file.Size > MaxFileSize)
        {
            Snackbar.Add($"File size exceeds the maximum allowed size of {MaxFileSize / (1024 * 1024)}MB", Severity.Error);
            StateHasChanged();
            return;
        }

        _selectedFile = file;
        _selectedFileName = file.Name;

        try
        {
            using var stream = file.OpenReadStream(MaxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var buffer = memoryStream.ToArray();
            var base64 = Convert.ToBase64String(buffer);
            _previewUrl = $"data:{file.ContentType};base64,{base64}";
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading preview: {ex.Message}", Severity.Error);
            _selectedFile = null;
            _selectedFileName = null;
        }

        StateHasChanged();
    }

    private async Task Upload()
    {
        if (_selectedFile is null)
        {
            return;
        }

        _isUploading = true;
        StateHasChanged();

        try
        {
            using var fileStream = _selectedFile.OpenReadStream(MaxFileSize);
            using var memoryStream = new MemoryStream();

            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var result = await UsersService.UploadAvatarAsync(
                UserId,
                memoryStream,
                _selectedFile.Name,
                _selectedFile.ContentType
            );

            if (!result.IsSuccess)
            {
                Snackbar.Add($"Upload failed: {result.ErrorMessage}", Severity.Error);
                return;
            }

            Snackbar.Add("Profile photo updated successfully", Severity.Success);

            MudDialog.Close(DialogResult.Ok(true));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Upload failed: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isUploading = false;
            StateHasChanged();
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
