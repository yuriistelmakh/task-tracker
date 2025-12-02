using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

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

                var connectionString = configuration.GetConnectionString("TestConnection");

                EnsureDatabase.For.SqlDatabase(connectionString);

                var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    logger.LogError(result.Error, "An error occurred while migrating.");
                    throw result.Error;
                }

                logger.LogInformation("Database migrated successfully!");
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
