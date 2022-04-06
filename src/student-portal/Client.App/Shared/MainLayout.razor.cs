using Client.App.Pages.Category.Modals;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Shared
{
    public partial class MainLayout : IAsyncDisposable
    {
        private bool IsAuthenticated { get; set; }
        private bool DrawerOpen { get; set; } = true;
        private MudTheme CurrentTheme { get; set; }

        protected override void OnInitialized()
        {
            CurrentTheme = _clientPreferenceManager.GetCurrentTheme();
            FetchDataExecutor.StartExecuting();
            RenderUIExecutor.StartExecuting();
        }

        protected async override Task OnInitializedAsync()
        {
            await AppBreakpointService.InitAsync();
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            IsAuthenticated = user != null && user.Identity?.IsAuthenticated == true;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
                StateHasChanged();

                if (!IsAuthenticated)
                {
                    _navigationManager.NavigateTo("/", true);
                }

                await InvokeCategoryPreferencesAsync();
            }
        }

        private async Task InvokeCategoryPreferencesAsync()
        {
            var preferences = await CategoryManager.FetchPreferencesAsync();

            if (!preferences.Any())
            {
                _dialogService.Show<SetCategoryPreferencesModal>("Update Interest");
            }
        }

        public async ValueTask DisposeAsync()
        {
            await AppBreakpointService.DisposeAsync();
        }
    }
}