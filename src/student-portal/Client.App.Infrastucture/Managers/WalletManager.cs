using Application.Common.Dtos;
using Application.Common.Models;
using Client.App.Infrastructure.WebServices;
using Client.Infrastructure.Constants;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class WalletManager : ManagerBase, IWalletManager
    {
        private readonly IWalletWebService _walletWebService;
        public WalletManager(IManagerToolkit managerToolkit, IWalletWebService walletWebService) : base(managerToolkit)
        {
            _walletWebService = walletWebService;
        }

        public async Task<IResult<WalletInfoDto>> GetWalletInfoAsync()
        {
            await PrepareForWebserviceCall();
            var response = await _walletWebService.GetWalletInfoAsync(AccessToken);
            if (response.Succeeded)
            {
                await ManagerToolkit.SaveDataAsync(StorageConstants.Local.Wallet, response.Data);
            }
            return response;
        }

        public Task<WalletInfoDto> FetchWalletInfoAsync() => ManagerToolkit.GetDataAsync<WalletInfoDto>(StorageConstants.Local.Wallet);
    }
}
