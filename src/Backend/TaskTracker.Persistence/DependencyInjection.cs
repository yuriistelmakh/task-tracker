using Dapper;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Enums;
using TaskTracker.Persistence.UoW;

namespace TaskTracker.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new StringEnumHandler<Roles>());
        SqlMapper.AddTypeHandler(new StringEnumHandler<BoardRoles>());

        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        return services;
    }
}
