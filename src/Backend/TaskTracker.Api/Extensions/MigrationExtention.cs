using DbMigrator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace TaskTracker.Api.Extensions;

public static class MigrationExtension
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Starting database migration...");

                var connectionString = configuration.GetConnectionString("TestConnection")!;
                DatabaseInitializer.Initialize(connectionString, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating.");
                throw;
            }
        }

        return host;
    }
}
