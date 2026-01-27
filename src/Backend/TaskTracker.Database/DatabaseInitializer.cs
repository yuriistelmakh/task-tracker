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
                INSERT INTO Users (Email, PasswordHash, Tag, DisplayName, Role, CreatedAt, IsDeleted, AvatarUrl)
                VALUES 
                    ('admin@task.com', @h, 'admin', N'Адміністратор', 'Admin', GETDATE(), 0, 'https://yuriitasktracker.blob.core.windows.net/avatars/avatar_fallback.jpg'),
                    ('dev@task.com', @h, 'developer', N'Розробник', 'User', GETDATE(), 0, 'https://yuriitasktracker.blob.core.windows.net/avatars/avatar_fallback.jpg'),
                    ('qa@task.com', @h, 'tester', N'Тестувальник', 'User', GETDATE(), 0, 'https://yuriitasktracker.blob.core.windows.net/avatars/avatar_fallback.jpg'),
                    ('manager@task.com', @h, 'manager', N'Менеджер', 'User', GETDATE(), 0, 'https://yuriitasktracker.blob.core.windows.net/avatars/avatar_fallback.jpg'),
                    ('design@task.com', @h, 'designer', N'Дизайнер', 'User', GETDATE(), 0, 'https://yuriitasktracker.blob.core.windows.net/avatars/avatar_fallback.jpg'),
                    ('analyst@task.com', @h, 'analyst', N'Аналітик', 'User', GETDATE(), 0, 'https://yuriitasktracker.blob.core.windows.net/avatars/avatar_fallback.jpg')",
                new { h = passHash });

            var boardId = await db.QuerySingleAsync<int>(@"
            INSERT INTO Boards (Title, Description, BackgroundColor, CreatedBy, CreatedAt, IsArchived) 
            VALUES (N'Головна дошка', N'Основний простір для задач', '#90AB8B', 1, GETUTCDATE(), 0);
            SELECT CAST(SCOPE_IDENTITY() as int);");

            await db.ExecuteAsync(@"
            INSERT INTO BoardMembers (UserId, BoardId, Role, JoinedAt) 
            VALUES 
                (1, @bid, 'Owner', GETUTCDATE()),
                (2, @bid, 'Member', GETUTCDATE()),
                (3, @bid, 'Member', GETUTCDATE()),
                (4, @bid, 'Admin', GETUTCDATE()),
                (6, @bid, 'Member', GETUTCDATE())", new { bid = boardId });

            await db.ExecuteAsync(@"
            INSERT INTO [Columns] (BoardId, Title, [Order], CreatedBy, CreatedAt) 
            VALUES (@bid, N'В черзі', 1, 1, GETUTCDATE()),
                   (@bid, N'В процесі', 2, 1, GETUTCDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);", new { bid = boardId });

            await db.ExecuteAsync(@"
            INSERT INTO Tasks (ColumnId, Title, Priority, [Order], CreatedBy, AssigneeId, IsComplete, CreatedAt) 
            VALUES 
                (1, N'Налаштувати проект', 2, 1, 1, 1, 1, GETUTCDATE()), 
                (1, N'Розробити API для дощок', 0, 2, 1, 2, 0, GETUTCDATE()),
                (2, N'Написати документацію до API', 2, 3, 1, 1, 0, GETUTCDATE()),
                (2, N'Налаштувати CI/CD пайплайн', 0, 4, 1, 1, 0, GETUTCDATE()),
                (1, N'Наповнити базу тестовими даними', 1, 5, 1, 1, 0, GETUTCDATE()),
                (1, N'Намалювати макет головної сторінки', 2, 6, 1, 1, 0, GETUTCDATE())");

            await db.ExecuteAsync(@"
            INSERT INTO Comments (TaskId, Content, CreatedBy, CreatedAt)
            VALUES 
                (1, N'Почав налаштування проєкту. Перевірте, чи всі пакети підтягнулись.', 2, DATEADD(MINUTE, -75, GETUTCDATE())),
                (1, N'Додав базову структуру solution та проєкти.', 2, DATEADD(MINUTE, -72, GETUTCDATE())),
                (1, N'Потрібно узгодити назви неймспейсів перед тим як рухатись далі.', 4, DATEADD(MINUTE, -70, GETUTCDATE())),
                (1, N'Ок, неймспейси залишаємо як зараз.', 1, DATEADD(MINUTE, -69, GETUTCDATE())),
                (1, N'Додав базові конфіги для appsettings та логування.', 2, DATEADD(MINUTE, -66, GETUTCDATE())),
                (1, N'Я протестував запуск локально — працює.', 3, DATEADD(MINUTE, -62, GETUTCDATE())),
                (1, N'Потрібно ще додати міграції/скрипти БД в CI.', 4, DATEADD(MINUTE, -58, GETUTCDATE())),
                (1, N'Зроблю сьогодні. Також додам базовий seed для демо.', 2, DATEADD(MINUTE, -54, GETUTCDATE())),
                (1, N'Чи будемо використовувати Dapper всюди чи місцями EF?', 6, DATEADD(MINUTE, -50, GETUTCDATE())),
                (1, N'Для цього проєкту — Dapper, щоб було простіше й швидше.', 1, DATEADD(MINUTE, -47, GETUTCDATE())),
                (1, N'Ок, тоді я підготую шаблони репозиторіїв.', 6, DATEADD(MINUTE, -44, GETUTCDATE())),
                (1, N'Підкажіть, чи потрібен code style/formatting на pre-commit?', 3, DATEADD(MINUTE, -40, GETUTCDATE())),
                (1, N'Так, додайте хоча б dotnet format в pipeline.', 4, DATEADD(MINUTE, -37, GETUTCDATE())),
                (1, N'Додав pipeline скелет. Поки що без деплою.', 2, DATEADD(MINUTE, -32, GETUTCDATE())),
                (1, N'Супер. Можна вже демонструвати нескінченний скрол коментарів.', 1, DATEADD(MINUTE, -28, GETUTCDATE()));
            ");

            logger.LogInformation("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during data seeding.");
            throw;
        }
    }
}