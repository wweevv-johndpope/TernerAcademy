using System.Threading.Tasks;

namespace Client.App.Services
{
    public interface IAccountService
    {
        Task<bool> ValidateAuthorizedSessionAsync();
    }
}