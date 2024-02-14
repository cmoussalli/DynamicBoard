using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;


namespace DynamicBoard.Application.DomainServices
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Implement the logic to retrieve the current authentication state
            var user = await GetCurrentAuthenticatedUser();

            // Create a new authentication state with the user
            var authState = new AuthenticationState(user);

            return authState;
        }

        public async Task SetAuthenticationStateAsync(ClaimsPrincipal user)
        {
            // Implement the logic to set the authentication state
            var authState = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        private Task<ClaimsPrincipal> GetCurrentAuthenticatedUser()
        {
            // Implement the logic to retrieve the current authenticated user

            // For testing purposes, you can create a dummy ClaimsPrincipal
            var identity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.Name, "John Doe"),
        // Add any additional claims
    }, "CustomAuthenticationType");

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(user);
        }
    }
}
