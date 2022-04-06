using Application.Common.Dtos;
using Application.Common.Models;
using Application.InstructorPortal.Wallets.Commands.Update;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class WalletWebService : WebServiceBase, IWalletWebService
    {
        public WalletWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<WalletInfoDto>> GetWalletInfoAsync(string accessToken) => GetAsync<WalletInfoDto>(WalletEndpoints.GetWalletInfo, accessToken);
        public Task<IResult> UpdateWalletAsync(UpdateWalletCommand request, string accessToken) => PostAsync(WalletEndpoints.UpdateWallet, request, accessToken);
    }
}
