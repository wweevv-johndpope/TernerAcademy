using Application.Common.Models;
using Application.StudentPortal.Account.Dtos;
using Blazored.LocalStorage;
using Client.Infrastructure.Constants;
using Client.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class ManagerToolkit : IManagerToolkit
    {
        private readonly ILocalStorageService _localStorage;

        public ManagerToolkit(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveAuthTokenHandler(AuthTokenHandler authTokenHandler) => await _localStorage.SetItemAsync(StorageConstants.Local.AuthTokenHandler, authTokenHandler);

        public async Task ClearAuthTokenHandler()
        {
            await _localStorage.ClearAsync();
        }

        public async Task<string> GetAuthToken()
        {
            var tokenHandler = await _localStorage.GetItemAsync<AuthTokenHandler>(StorageConstants.Local.AuthTokenHandler);

            if (tokenHandler == null || !tokenHandler.IsValid())
            {
                await ClearAuthTokenHandler();
                throw new SessionExpiredException();
            }

            return tokenHandler.Token;
        }

        public async Task SaveProfile(StudentMyProfileDto data) => await _localStorage.SetItemAsync(StorageConstants.Local.Profile, data);
        public async Task<StudentMyProfileDto> GetProfile()
        {
            var data = await _localStorage.GetItemAsync<StudentMyProfileDto>(StorageConstants.Local.Profile);
            return data;
        }

        public async Task SaveDataAsync<T>(string key, T data) => await _localStorage.SetItemAsync(key, data);
        public async Task<T> GetDataAsync<T>(string key)
        {
            var data = await _localStorage.GetItemAsync<T>(key);
            return data;
        }
    }
}
