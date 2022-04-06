using Application.Common.Dtos;
using Application.Common.Models;
using Application.General.Queries.GetCourseData;
using Client.App.Infrastructure.WebServices;
using Client.Infrastructure.Constants;
using Domain.Views;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class GeneralManager : ManagerBase, IGeneralManager
    {
        private readonly IGeneralWebService _generalWebService;

        public GeneralManager(IManagerToolkit managerToolkit, IGeneralWebService generalWebService) : base(managerToolkit)
        {
            _generalWebService = generalWebService;
        }

        public async Task<IResult<GetCourseDataResponseDto>> GetCourseDataAsync()
        {
            var response = await _generalWebService.GetCourseDataAsync();
            if (response.Succeeded)
            {
                await ManagerToolkit.SaveDataAsync(StorageConstants.Local.CourseCategories, response.Data.CategoryData.OrderBy(x => x.Category).ThenBy(x => x.Topic).ToList());
                await ManagerToolkit.SaveDataAsync(StorageConstants.Local.CourseLanguages, response.Data.LanguageData);
            }
            return response;
        }

        public Task<List<CategoryTopicViewItem>> FetchCourseCategoriesAsync() => ManagerToolkit.GetDataAsync<List<CategoryTopicViewItem>>(StorageConstants.Local.CourseCategories);
        public Task<List<CourseLanguageDto>> FetchCourseLanguagesAsync() => ManagerToolkit.GetDataAsync<List<CourseLanguageDto>>(StorageConstants.Local.CourseLanguages);

        public async Task<IResult<string>> GetTxAsync(string txHash)
        {
            return await _generalWebService.GetTxAsync(txHash);
        }

        public async Task<IResult<double>> GetTFuelPriceAsync()
        {
            var response = await _generalWebService.GetTFuelPriceAsync();
            if (response.Succeeded)
            {
                await ManagerToolkit.SaveDataAsync(StorageConstants.Local.TFuelPrice, response.Data);
            }
            return response;
        }

        public Task<double> FetchTFuelPriceAsync() => ManagerToolkit.GetDataAsync<double>(StorageConstants.Local.TFuelPrice);
    }
}
