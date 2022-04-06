using Application.Common.Dtos;
using Application.Common.Models;
using Application.InstructorPortal.Wallets.Commands.Update;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IWalletManager : IManager
    {
        Task<IResult<WalletInfoDto>> GetWalletInfoAsync();
        Task<WalletInfoDto> FetchWalletInfoAsync();
        Task<IResult> UpdateWalletAsync(UpdateWalletCommand request);
    }
}