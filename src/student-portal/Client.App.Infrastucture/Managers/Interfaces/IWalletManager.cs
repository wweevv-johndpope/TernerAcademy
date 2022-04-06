using Application.Common.Dtos;
using Application.Common.Models;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IWalletManager : IManager
    {
        Task<IResult<WalletInfoDto>> GetWalletInfoAsync();
        Task<WalletInfoDto> FetchWalletInfoAsync();
    }
}