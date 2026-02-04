using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Refit;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services;
using TaskTracker.WebApp.Components;
using TaskTracker.Services.Auth;
using TaskTracker.WebApp.Components.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddProjectServices();

builder.Services.AddTransient<AuthHeaderHandler>();

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer()
};

builder.Services.AddRefitClient<IAuthApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]));

builder.Services.AddRefitClient<IBoardsApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddRefitClient<ITasksApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddRefitClient<IColumnsApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddRefitClient<IUsersApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddRefitClient<IBoardMembersApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddRefitClient<ICommentsApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]))
    .AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<UiStateService>();

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