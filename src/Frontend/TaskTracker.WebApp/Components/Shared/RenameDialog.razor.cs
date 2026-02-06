using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace TaskTracker.WebApp.Components.Shared;

public partial class RenameDialog
{
    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = "Rename";

    [Parameter]
    public string CurrentName { get; set; } = string.Empty;

    private string _newName = string.Empty;

    protected override void OnInitialized()
    {
        _newName = CurrentName;
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private void Submit()
    {
        if (!string.IsNullOrWhiteSpace(_newName))
        {
            MudDialog.Close(DialogResult.Ok(_newName));
        }
    }

    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !string.IsNullOrWhiteSpace(_newName))
        {
            Submit();
        }
        else if (e.Key == "Escape")
        {
            Cancel();
        }
    }
}
