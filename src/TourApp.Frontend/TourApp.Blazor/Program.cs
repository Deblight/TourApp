using TourApp.Application.DepenedencyInjection;
using TourApp.Blazor.Components;
using TourApp.Blazor.Interfaces;
using TourApp.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// builder.Services.AddEventApiDemo();
builder.Services.AddScoped<IEventApi, RabbitMQEventApi>();
builder.Services.AddBookingService();
//NOTE: currently hardcoded, should be read from launchSettings.json
builder.Services.AddDatabaseConnection("localhost", "user", "password", 52001);


var app = builder.Build();

// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error", createScopeForErrors: true);
//     app.UseHsts();
// }

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
