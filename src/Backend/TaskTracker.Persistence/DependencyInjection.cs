using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Persistence.Repositories;

namespace TaskTracker.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IBoardRepository, BoardRepository>();

            return services;
        }
    }
}
