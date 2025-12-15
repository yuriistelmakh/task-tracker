using TaskTracker.WebApp.Components;
using Refit;
using TaskTracker.Services.Abstraction.Interfaces;
using MudBlazor.Services;
using TaskTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddMudServices();

builder.Services.AddRefitClient<IAuthApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7275"));

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();
