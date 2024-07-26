using Giggle.Configurations;
using Giggle.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

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
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenService.GetTokenValidationParameters();
            });

            services.AddAuthorization(options =>
            {
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