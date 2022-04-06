using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Authentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Client.App.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppStateProvider _stateProvider;
        private readonly NavigationManager _navigationManager;
        private readonly IAccountManager _accountManager;
        private readonly ISnackbar _snackbar;
        private readonly IExceptionHandler _exceptionHandler;

        public AccountService(AppStateProvider stateProvider, NavigationManager navigationManager, ISnackbar snackbar, IAccountManager accountManager, IExceptionHandler exceptionHandler)
        {
            _stateProvider = stateProvider;
            _navigationManager = navigationManager;
            _snackbar = snackbar;
            _accountManager = accountManager;
            _exceptionHandler = exceptionHandler;
        }

        public async Task<bool> ValidateAuthorizedSessionAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            if (state.User == null || state.User?.Identity.IsAuthenticated == false)
            {
                _navigationManager.NavigateTo("/auth/login");
                return false;
            }

            return true;
        }
    }
}
