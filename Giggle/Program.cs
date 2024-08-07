using DotNetEnv;
using Giggle.Configurations;
using Giggle.Helpers;
using Giggle.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Services.AddHttpContextAccessor();
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

// Configure logging
builder.Services.AddLogging(loggingBuilder => {
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
//CORS settings allow credentials and headers necessary for authentication
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", builder =>
    {
        builder.WithOrigins(Environment.GetEnvironmentVariable("APP_ORGINE"))
               .AllowCredentials()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    app.UseMiddleware<NotFoundMiddleware>();
    app.UseExceptionHandler("/api/error/error");
    app.UseHsts();
    app.UseStatusCodePagesWithReExecute("/Error/NotFound");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


if (Environment.GetEnvironmentVariable("APP_DEBUG") == "TRUE")
{
    app.Use(async (context, next) =>
    {
        var user = context.User;
        if (user.Identity.IsAuthenticated)
        {
            var claims = user.Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }
        }
        else
        {
            Console.WriteLine("User is not authenticated");
        }
        await next();
    });

}
//else
//{
//    app.Use(async (context, next) =>
//    {
//        if (!context.User.Identity.IsAuthenticated)
//        {
//            context.Response.Redirect("/auth/login");
//        }
//        else
//        {
//            await next();
//        }
//    });
//}

// Map routes for controllers and Razor pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Map Blazor SignalR hub
app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");
//app.MapRazorPages();
app.Run();
