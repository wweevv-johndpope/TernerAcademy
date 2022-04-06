using Application.Common.Dtos;
using Client.App.Infrastructure.Managers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Shared.Components
{
    public partial class AppBarContent : IDisposable
    {
        [Inject] public IWalletManager WalletManager { get; set; }

        public string Name { get; set; }

        public string ShortenWalletAddress { get; set; }
        public string WalletAddress { get; set; }
        public string WalletBalance { get; set; }

        public double TFuelPrice { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            RenderUIExecutor.FetchWalletInfoJobExecuted += HandleFetchWalletInfoJobExecuted;
            RenderUIExecutor.FetchTFuelPriceJobExecuted += HandleFetchTFuelPriceJobExecuted;
        }

        public void Dispose()
        {
            RenderUIExecutor.FetchWalletInfoJobExecuted -= HandleFetchWalletInfoJobExecuted;
            RenderUIExecutor.FetchTFuelPriceJobExecuted -= HandleFetchTFuelPriceJobExecuted;
        }

        private async void HandleFetchWalletInfoJobExecuted(object sender, WalletInfoDto e)
        {
            RenderWallet(e);
            await InvokeAsync(StateHasChanged);
        }

        private async void HandleFetchTFuelPriceJobExecuted(object sender, double e)
        {
            TFuelPrice = e;
            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadDataAsync()
        {
            var profile = await AccountManager.FetchProfileAsync();

            if (profile != null)
            {
                Name = profile.Name;
            }

            var wallet = await WalletManager.FetchWalletInfoAsync();

            if (wallet != null)
            {
                RenderWallet(wallet);
            }

            TFuelPrice = await GeneralManager.FetchTFuelPriceAsync();

            await InvokeAsync(StateHasChanged);
        }

        private void RenderWallet(WalletInfoDto wallet)
        {
            if (!string.IsNullOrEmpty(wallet.Address))
                ShortenWalletAddress = string.Format("{0}....{1}", wallet.Address.Substring(0, 5), wallet.Address.Substring(wallet.Address.Length - 5, 5));

            WalletAddress = wallet.Address;
            WalletBalance = $"{wallet.Balance:N3}";
        }

        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), "Are you sure you want to logout?"},
                {nameof(Dialogs.Logout.ButtonText), "Logout"},
                {nameof(Dialogs.Logout.Color), Color.Error},
            };

            _dialogService.Show<Dialogs.Logout>("Logout", parameters);
        }
    }
}