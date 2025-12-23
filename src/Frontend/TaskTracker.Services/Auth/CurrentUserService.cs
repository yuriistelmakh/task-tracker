using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly AuthenticationStateProvider _authProvider;

    public CurrentUserService(AuthenticationStateProvider authProvider)
    {
        _authProvider = authProvider;
    }

    public async Task<bool> IsUserAuthenticated()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        return state.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string?> GetUserDisplayName()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        return state.User.FindFirst(ClaimTypes.Name)?.Value;
    }

    public async Task<int?> GetUserId()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        var userIdString = state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(userIdString, out var userId) ? userId : null;
    }
}
