using Application.Common.Dtos;
using Client.App.Infrastructure.Managers;
using System;
using System.Timers;

namespace Client.App.PeriodicExecutors
{
    public class RenderUIExecutor : IDisposable
    {
        private readonly IWalletManager _walletManager;
        private readonly IGeneralManager _generalManager;
        private Timer _timer;
        private bool _running;

        public RenderUIExecutor(IWalletManager walletManager, IGeneralManager generalManager)
        {
            _walletManager = walletManager;
            _generalManager = generalManager;
        }

        public event EventHandler<WalletInfoDto> FetchWalletInfoJobExecuted;
        public event EventHandler<double> FetchTFuelPriceJobExecuted;

        public void StartExecuting()
        {
            if (!_running)
            {
                _timer = new Timer();
                _timer.Interval = 3000;
                _timer.Elapsed += HandleTimer;
                _timer.AutoReset = true;
                _timer.Enabled = true;
                _running = true;
            }
        }

        async void HandleTimer(object source, ElapsedEventArgs e)
        {
            try
            {
                var wallet = await _walletManager.FetchWalletInfoAsync();

                if (wallet != null)
                {
                    FetchWalletInfoJobExecuted?.Invoke(this, wallet);
                }

                var tFuelPrice = await _generalManager.FetchTFuelPriceAsync();
                FetchTFuelPriceJobExecuted?.Invoke(this, tFuelPrice);
            }
            catch
            {
                Console.WriteLine($"Fetch Wallet Executor: Fetch Error");
            }
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
