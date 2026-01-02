using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using MudBlazor;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.WebApp.Components.Pages.Home;

public partial class CreateBoardDialog : IComponent
{
    [CascadingParameter]
    public IMudDialogInstance Dialog { get; set; } = default!;

    [Inject]
    public IBoardsService BoardsService { get; set; } = default!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = default!;

    private string _title = string.Empty;

    private readonly List<string> _backgroundColorOptions = [
        "#5A7863",
        "#90AB8B",
        "#3B4953",
        "#4E868E",
        "#6D8EA0",
        "#7986CB",
        "#9E8FB2",
        "#C27BA0",
        "#D0887A",
        "#D4A353",
        "#8D6E63",
        "#546E7A"
    ];

    private string _selectedColor = "#5A7863";

    private BoardVisibility _selectedVisibility = BoardVisibility.Private;

    private void OnColorClick(string newColor)
    {
        _selectedColor = newColor;
    }

    private void OnVisibilityOptionClicked(BoardVisibility option)
    {
        _selectedVisibility = option;
    }

    private void OnCloseClick()
    {
        Dialog.Close();
    }

    private async Task OnCreateClicked()
    {
        var request = new CreateBoardRequest
        {
            Title = _title,
            DisplayColor = _selectedColor,
            Visibility = _selectedVisibility
        };

        var result = await BoardsService.CreateAsync(request);
        
        if (!result.IsSuccess)
        {
            Snackbar.Add($"Error creating a new board: {result.ErrorMessage}");
            return;
        }

        Dialog.Close(DialogResult.Ok(result.Value));
    }
}
