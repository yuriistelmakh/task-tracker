using TaskTracker.Api.Extensions;
using TaskTracker.Application;
using TaskTracker.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddControllers();


var app = builder.Build();

app.MigrateDatabase();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
