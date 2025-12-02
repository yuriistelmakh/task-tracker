using DbUp;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DbMigrator;

public class DatabaseInitializer
{
    public static void Initialize(string connectionString, ILogger logger)
    {
        logger.LogInformation("Starting database migration...");

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                logger.LogError(result.Error, "An error occurred while migrating.");
                throw result.Error;
            }

            logger.LogInformation("Database migrated successfully!");
        }
        else
        {
            logger.LogInformation("Database is up to date");
        }
    }
}
