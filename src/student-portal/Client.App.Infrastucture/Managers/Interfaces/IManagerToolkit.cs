using Application.Common.Models;
using Application.StudentPortal.Account.Dtos;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IManagerToolkit : IManager
    {
        Task ClearAuthTokenHandler();
        Task SaveAuthTokenHandler(AuthTokenHandler authTokenHandler);
        Task<string> GetAuthToken();

        Task SaveProfile(StudentMyProfileDto data);
        Task<StudentMyProfileDto> GetProfile();

        Task SaveDataAsync<T>(string key, T data);
        Task<T> GetDataAsync<T>(string key);
    }
}