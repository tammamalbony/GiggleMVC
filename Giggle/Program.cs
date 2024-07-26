using DotNetEnv;
using Giggle.Configurations;
using Giggle.Helpers;
using Giggle.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add blazor
builder.Services.AddBlazor();

// Create an instance of CustomConfigurationManager
var configManager = new CustomConfigurationManager(builder.Configuration);

// Register services and repositories using the helper method with the configManager instance
ServiceRegistrationHelper.RegisterServicesAndRepositories(builder.Services, configManager);

// Configure JWT Authentication using the helper method with the configManager instance
JwtHelper.ConfigureJwtAuthentication(builder.Services, configManager);

// Configure Identity using the helper method
builder.Services.ConfigureIdentity();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    app.UseExceptionHandler("/api/error/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



// Map routes for controllers and Razor pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Map Blazor SignalR hub
app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");
//app.MapRazorPages();
app.Run();
