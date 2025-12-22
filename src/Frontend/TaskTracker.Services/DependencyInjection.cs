using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.Services.Auth;

namespace TaskTracker.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ISessionCacheService, SessionCacheService>();

        services.AddTransient<AuthHeaderHandler>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBoardsService, BoardsService>();

        services.AddScoped<CustomAuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => 
            sp.GetRequiredService<CustomAuthStateProvider>());

        return services;
    }
}