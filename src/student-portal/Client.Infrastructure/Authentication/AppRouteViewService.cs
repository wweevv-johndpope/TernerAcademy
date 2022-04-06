using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Client.Infrastructure.Authentication
{
    public class AppRouteViewService
    {
        private readonly AppStateProvider _stateProvider;

        public AppRouteViewService(AppStateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public bool IsAuthenticated { get; private set; }

        public async Task Populate()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            SetAuthenticatedStatus(state);
        }

        private void SetAuthenticatedStatus(AuthenticationState state)
        {
            IsAuthenticated = state.User != null && state.User?.Identity.IsAuthenticated == true;
        }
    }
}
