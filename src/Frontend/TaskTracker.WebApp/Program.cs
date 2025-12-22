using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using Refit;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services;
using TaskTracker.WebApp.Components;
using TaskTracker.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddProjectServices();

builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services.AddRefitClient<IAuthApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]));

builder.Services.AddRefitClient<IBoardsApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddRefitClient<ITasksApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();