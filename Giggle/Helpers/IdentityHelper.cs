using Giggle.Identity;
using Giggle.Identity.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Giggle.Helpers
{
    public static class IdentityHelper
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            //services.AddSingleton<IAuthorizationHandler, HasClaimHandler>();
            //services.AddSingleton<IUserStore<IdentityUser>, CustomUserStore>();
            //services.AddSingleton<IUserEmailStore<IdentityUser>, CustomUserStore>();
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddDefaultTokenProviders();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    // Password settings
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = true;
            //});

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                options.LoginPath = "/auth/Login";
                options.AccessDeniedPath = "/auth/AccessDenied";
                options.SlidingExpiration = true;
            });
        }
    }
}
