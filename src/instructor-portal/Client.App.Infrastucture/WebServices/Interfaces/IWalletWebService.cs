using Application.Common.Dtos;
using Application.Common.Models;
using Application.InstructorPortal.Wallets.Commands.Update;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IWalletWebService : IWebService
    {
        Task<IResult<WalletInfoDto>> GetWalletInfoAsync(string accessToken);
        Task<IResult> UpdateWalletAsync(UpdateWalletCommand request, string accessToken);
    }
}