using Giggle.Providers;

namespace Giggle.Helpers
{
    public static class BlazorServicesHelper
    {
        public static void AddBlazor(this IServiceCollection services)
        {
            services.AddRazorPages(options =>
			{
				//options.RootDirectory = "/Views/";
			});
			services.AddServerSideBlazor();
            services.AddSignalR();

            // Register BaseAddressProvider
            services.AddScoped<BaseAddressProvider>();

            // Register HttpClient
            services.AddScoped(sp =>
            {
                var baseAddressProvider = sp.GetRequiredService<BaseAddressProvider>();
                return new HttpClient { BaseAddress = new Uri(baseAddressProvider.GetBaseAddress()) };
            });
        }
    }
}
