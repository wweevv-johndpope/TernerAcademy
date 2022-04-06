using Application.Common.Dtos;
using Application.Common.Models;
using Application.General.Queries.GetCourseData;
using Domain.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IGeneralManager : IManager
    {
        Task<IResult<GetCourseDataResponseDto>> GetCourseDataAsync();
        Task<List<CategoryTopicViewItem>> FetchCourseCategoriesAsync();
        Task<List<CourseLanguageDto>> FetchCourseLanguagesAsync();
        Task<IResult<string>> GetTxAsync(string txHash);
        Task<IResult<double>> GetTFuelPriceAsync();
        Task<double> FetchTFuelPriceAsync();
    }
}