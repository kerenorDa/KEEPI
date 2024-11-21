using Keepi.Shared;
using Microsoft.JSInterop;

namespace Keepi.Client.Services
{
    public class AuthService
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> IsUserLoggedIn()
        {
            var user = await _jsRuntime.InvokeAsync<User>("localStorageHelper.get", "user");
            return user != null;
        }
    }
}
