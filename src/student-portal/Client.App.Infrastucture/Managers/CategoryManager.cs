using Application.Common.Dtos;
using Application.Common.Models;
using Application.StudentPortal.Categories.Commands.SetPreferences;
using Blazored.LocalStorage;
using Client.App.Infrastructure.WebServices;
using Client.Infrastructure.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CategoryManager : ManagerBase, ICategoryManager
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ICategoryWebService _categoryWebService;

        public CategoryManager(IManagerToolkit managerToolkit, ILocalStorageService localStorage, ICategoryWebService categoryWebService) : base(managerToolkit)
        {
            _localStorage = localStorage;
            _categoryWebService = categoryWebService;
        }

        private async Task SavePreferences(List<CategoryDto> data) => await _localStorage.SetItemAsync(StorageConstants.Local.CategoryPreferences, data);

        public async Task<List<CategoryDto>> FetchPreferencesAsync()
        {
            var data = await _localStorage.GetItemAsync<List<CategoryDto>>(StorageConstants.Local.CategoryPreferences);
            data ??= new();
            return data;
        }

        public async Task<IResult<List<CategoryDto>>> GetPreferencesAsync()
        {
            await PrepareForWebserviceCall();
            var response = await _categoryWebService.GetPreferencesAsync(AccessToken);
            if (response.Succeeded)
            {
                await SavePreferences(response.Data);
            }

            return response;
        }

        public async Task<IResult> SetPreferencesAsync(SetCategoryPreferencesCommand request)
        {
            await PrepareForWebserviceCall();
            return await _categoryWebService.SetPreferencesAsync(request, AccessToken);
        }


    }
}
