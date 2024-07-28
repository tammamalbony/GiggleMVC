using Giggle.Configurations;
using Giggle.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Giggle.Helpers
{
    public static class JwtHelper
    {
        public static void ConfigureJwtAuthentication(IServiceCollection services, CustomConfigurationManager configManager)
        {
            var tokenService = new TokenService(configManager);


            services.AddSingleton(tokenService);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenService.GetTokenValidationParameters();
                options.TokenHandlers.Add(new JwtSecurityTokenHandler());
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("jwt"))
                        {
                            var token = context.Request.Cookies["jwt"];
                            if (Environment.GetEnvironmentVariable("APP_DEBUG") == "TRUE")
                            {
                                Console.WriteLine($"Received JWT: {token}");  // For debugging

                            }
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },
                     OnChallenge = context =>
                     {
                         context.HandleResponse();
                         context.Response.StatusCode = 401;
                         context.Response.ContentType = "application/json";
                         return context.Response.WriteAsync("You are not authorized.");
                     },
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine("Authentication failed: " + context.Exception.Message);
                         return Task.CompletedTask;
                     },
                    OnTokenValidated = c =>
                    {
                        if (c.Principal.IsInRole("Admin"))
                        {
                            c.HttpContext.Items["Admin"] = true;
                        }
                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                };
            }).AddCookie( options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = "/auth/login"; // Redirect here for unauthorized access
                options.AccessDeniedPath = "/auth/AccessDenied"; // Redirect here for forbidden access
                options.LogoutPath = "/auth/Logout";
                options.Events = new CookieAuthenticationEvents
                {
                    // Define the event handler for validating the cookie
                    OnValidatePrincipal = cookieValidationContext =>
                    {
                        var userService = cookieValidationContext.HttpContext.RequestServices.GetRequiredService<TokenService>();
                        var token = cookieValidationContext.Principal?.FindFirst("jwt")?.Value;

                        if (!userService.ValidateToken(token))
                        {
                            // Invalidate the cookie if the token validation fails
                            cookieValidationContext.RejectPrincipal();
                            cookieValidationContext.HttpContext.Response.Cookies.Delete(cookieValidationContext.Options.Cookie.Name);
                        }

                        return Task.CompletedTask;
                    }
                };
            }); 

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizedUser", policy => policy.RequireClaim(ClaimTypes.Name));
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("VerifiedUser", policy =>
                    policy.RequireClaim("IsVerified", "True"));
            });
        }
    }
}

/*
[Authorize(Policy = "VerifiedUser")]
public IActionResult SomeSecureAction()
{
    // Action logic
    return View();
}

@using Microsoft.AspNetCore.Authorization

@if (User.Identity.IsAuthenticated && User.HasClaim("IsVerified", "True"))
{
    <p>Welcome, verified user!</p>
}
else
{
    <p>Please verify your account.</p>
}

*/