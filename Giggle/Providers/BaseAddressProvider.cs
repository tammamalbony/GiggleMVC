using Microsoft.AspNetCore.Components;

namespace Giggle.Providers
{
    public class BaseAddressProvider
    {
        private readonly NavigationManager _navigationManager;

        public BaseAddressProvider(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public string GetBaseAddress()
        {
            return _navigationManager.BaseUri;
        }
    }
}
