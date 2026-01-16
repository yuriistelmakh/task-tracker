using DbUp;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskTracker.Database;

public class DatabaseInitializer
{
    public static async Task InitializeAsync(string connectionString, ILogger logger)
    {
        logger.LogInformation("Starting database migration...");

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

        await SeedDataAsync(connectionString, logger);
    }

    private static async Task SeedDataAsync(string connectionString, ILogger logger)
    {
        using IDbConnection db = new SqlConnection(connectionString);

        var usersExist = await db.ExecuteScalarAsync<bool>("SELECT TOP 1 1 FROM Users");
        if (usersExist) return;

        logger.LogInformation("Seeding initial data (Users 1–4, Board, Tasks)...");

        try
        {
            var passHash = "$2a$11$0OhwqhM/OTH0qYCyomUSoeKe0S4iQBHRDp.0L88TuM/PzMBlhVN96";

            await db.ExecuteAsync(@"
                INSERT INTO Users (Email, PasswordHash, Tag, DisplayName, Role, CreatedAt, IsDeleted)
                VALUES 
                    ('admin@task.com', @h, 'admin', N'Адміністратор', 'Admin', GETDATE(), 0),
                    ('dev@task.com', @h, 'developer', N'Розробник', 'User', GETDATE(), 0),
                    ('qa@task.com', @h, 'tester', N'Тестувальник', 'User', GETDATE(), 0),
                    ('manager@task.com', @h, 'manager', N'Менеджер', 'User', GETDATE(), 0),
                    ('design@task.com', @h, 'designer', N'Дизайнер', 'User', GETDATE(), 0),
                    ('analyst@task.com', @h, 'analyst', N'Аналітик', 'User', GETDATE(), 0)",
                new { h = passHash });

            var boardId = await db.QuerySingleAsync<int>(@"
            INSERT INTO Boards (Title, Description, BackgroundColor, CreatedBy, CreatedAt, IsArchived) 
            VALUES (N'Головна дошка', N'Основний простір для задач', '#90AB8B', 1, GETDATE(), 0);
            SELECT CAST(SCOPE_IDENTITY() as int);");

            await db.ExecuteAsync(@"
            INSERT INTO BoardMembers (UserId, BoardId, Role, JoinedAt) 
            VALUES 
                (1, @bid, 'Owner', GETDATE()),
                (2, @bid, 'Member', GETDATE()),
                (3, @bid, 'Member', GETDATE()),
                (4, @bid, 'Admin', GETDATE()),
                (6, @bid, 'Member', GETDATE())", new { bid = boardId });

            await db.ExecuteAsync(@"
            INSERT INTO [Columns] (BoardId, Title, [Order], CreatedBy, CreatedAt) 
            VALUES (@bid, N'В черзі', 1, 1, GETDATE()),
                   (@bid, N'В процесі', 2, 1, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);", new { bid = boardId });

            await db.ExecuteAsync(@"
            INSERT INTO Tasks (ColumnId, Title, Priority, [Order], CreatedBy, AssigneeId, IsComplete, CreatedAt) 
            VALUES 
                (1, N'Налаштувати проект', 2, 1, 1, 1, 1, GETDATE()), 
                (1, N'Розробити API для дощок', 0, 2, 1, 2, 0, GETDATE()),
                (2, N'Написати документацію до API', 2, 3, 1, 1, 0, GETDATE()),
                (2, N'Налаштувати CI/CD пайплайн', 0, 4, 1, 1, 0, GETDATE()),
                (1, N'Наповнити базу тестовими даними', 1, 5, 1, 1, 0, GETDATE()),
                (1, N'Намалювати макет головної сторінки', 2, 6, 1, 1, 0, GETDATE())");

            logger.LogInformation("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during data seeding.");
            throw;
        }
    }
}