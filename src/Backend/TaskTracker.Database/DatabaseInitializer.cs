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

        // 1. DbUp: Створення бази та міграції
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

        // 2. Dapper: Сідинг даних
        await SeedDataAsync(connectionString, logger);
    }

    private static async Task SeedDataAsync(string connectionString, ILogger logger)
    {
        using IDbConnection db = new SqlConnection(connectionString);

        // Перевіряємо, чи база порожня
        var usersExist = await db.ExecuteScalarAsync<bool>("SELECT TOP 1 1 FROM Users");
        if (usersExist) return;

        logger.LogInformation("Seeding initial data (Users 1–4, Board, Tasks)...");

        try
        {
            var passHash = "$2a$11$0OhwqhM/OTH0qYCyomUSoeKe0S4iQBHRDp.0L88TuM/PzMBlhVN96";

            // 1. Створюємо 4 користувачів (отримають ID 1, 2, 3, 4)
            await db.ExecuteAsync(@"
            INSERT INTO Users (Email, PasswordHash, Tag, DisplayName, Role, CreatedAt, IsDeleted)
            VALUES 
                ('admin@task.com', @h, 'admin', N'Адміністратор', 'Admin', GETDATE(), 0),
                ('dev@task.com', @h, 'developer', N'Розробник', 'User', GETDATE(), 0),
                ('qa@task.com', @h, 'tester', N'Тестувальник', 'User', GETDATE(), 0),
                ('manager@task.com', @h, 'manager', N'Менеджер', 'User', GETDATE(), 0)",
                new { h = passHash });

            // 2. Створюємо дошку (CreatedBy = 1)
            var boardId = await db.QuerySingleAsync<int>(@"
            INSERT INTO Boards (Title, Description, CreatedBy, CreatedAt, IsArchived) 
            VALUES (N'Головна дошка', N'Основний простір для задач', 1, GETDATE(), 0);
            SELECT CAST(SCOPE_IDENTITY() as int);");

            // 3. Додаємо всіх 4 користувачів до дошки
            await db.ExecuteAsync(@"
            INSERT INTO BoardMembers (UserId, BoardId, Role, JoinedAt) 
            VALUES 
                (1, @bid, 'Owner', GETDATE()),
                (2, @bid, 'Editor', GETDATE()),
                (3, @bid, 'Editor', GETDATE()),
                (4, @bid, 'Viewer', GETDATE())", new { bid = boardId });

            // 4. Створюємо колонку "В черзі"
            var columnId = await db.QuerySingleAsync<int>(@"
            INSERT INTO [Columns] (BoardId, Title, [Order], CreatedBy, CreatedAt) 
            VALUES (@bid, N'В черзі', 1, 1, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);", new { bid = boardId });

            // 5. Створюємо пару тестових завдань
            await db.ExecuteAsync(@"
            INSERT INTO Tasks (ColumnId, Title, Priority, [Order], CreatedBy, AssigneeId, IsComplete, CreatedAt) 
            VALUES 
                (@cid, N'Налаштувати проект', 3, 1, 1, 1, 1, GETDATE()), 
                (@cid, N'Розробити API для дощок', 2, 2, 1, 2, 0, GETDATE())",
                new { cid = columnId });

            logger.LogInformation("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during data seeding.");
            throw;
        }
    }
}