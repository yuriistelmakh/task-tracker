using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.BlobStorage;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Infrastructure.Auth;
using TaskTracker.Infrastructure.BlobStorage;
using TaskTracker.Infrastructure.Realtime;

namespace TaskTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IBoardNotificator, SignalRBoardNotificator>();
        services.AddSingleton<IOnlineBoardUsers, OnlineBoardUsers>();
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        return services;
    }
}
