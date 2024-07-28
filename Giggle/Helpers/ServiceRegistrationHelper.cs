using Giggle.Configurations;
using Giggle.Identity;
using Giggle.Providers;
using Giggle.Repositories;
using Giggle.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Giggle.Helpers
{
    public static class ServiceRegistrationHelper
    {
        public static void RegisterServicesAndRepositories(IServiceCollection services , CustomConfigurationManager configManager)
        {
            services.AddSingleton(configManager);


            // Register your custom helpers
            services.AddLogging(configure => configure.AddConsole())
                          .AddTransient(typeof(Logger<>));


            // Register your custom services
            services.AddTransient<TokenService>();
            services.AddScoped<AuthService>();
            services.AddTransient<DatabaseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasherServices, PasswordHasherServices>();

            // Register your custom repositories
            services.AddTransient<UserRepository>();
            services.AddTransient<RoleRepository>();

            // Register Identity services with the custom user store
            //services.AddScoped<IUserStore<IdentityUser>, CustomUserStore>();
            //services.AddScoped<IUserPasswordStore<IdentityUser>, CustomUserStore>();
            //services.AddScoped<IRoleStore<IdentityRole>, CustomRoleStore>();

        }
    }
}
