using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TaskTracker.WebApp.Components.Layout.Headers;

public partial class Header
{
    [Parameter]
    public RenderFragment? LeftContent { get; set; }

    [Parameter]
    public RenderFragment? RightContent { get; set; }

    [Parameter]
    public string SearchPlaceholder { get; set; } = "Search...";

    [Inject]
    public AuthenticationStateProvider AuthStateProvider { private get; set; } = default!;

    private string username = string.Empty;

    private string search = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        username = user.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";
    }
}
