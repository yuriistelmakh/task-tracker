using TaskTracker.Application;
using TaskTracker.Persistence;

namespace TaskTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSwaggerGen();

            builder.Services.AddApplication();
            builder.Services.AddPersistence();
            builder.Services.AddControllers();

            var app = builder.Build();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
