using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TaskTracker.WebApp.Components.Shared;

public partial class CustomDialog
{
    [Parameter]
    public Variant MainButtonVariant { get; set; }

    [Parameter]
    public Color MainButtonColor { get; set; }

    [Parameter]
    public required string Title { get; set; }

    [Parameter]
    public required string Description { get; set; }

    [Parameter]
    public required string MainButtonText { get; set; }

    [CascadingParameter]
    public IMudDialogInstance Dialog { get; set; } = default!;

    private void OnConfirm()
    {
        Dialog.Close(DialogResult.Ok(true));
    }

    private void OnCancel()
    {
        Dialog.Close();
    }
}
