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

    public async Task<string?> GetUserDisplayName()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        return state.User.FindFirst(ClaimTypes.Name)?.Value;
    }
}
