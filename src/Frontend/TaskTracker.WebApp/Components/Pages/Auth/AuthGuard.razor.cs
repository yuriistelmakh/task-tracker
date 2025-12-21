using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace TaskTracker.WebApp.Components.Pages.Auth;

public partial class AuthGuard
{
    [Parameter]
    public RenderFragment<AuthenticationState> ChildContent { get; set; } = default!;

    [Parameter]
    public string? Roles { get; set; }
}
