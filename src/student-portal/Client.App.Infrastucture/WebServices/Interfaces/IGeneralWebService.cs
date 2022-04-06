using Application.Common.Models;
using Application.General.Queries.GetCourseData;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IGeneralWebService : IWebService
    {
        Task<IResult<GetCourseDataResponseDto>> GetCourseDataAsync();
        Task<IResult<string>> GetTxAsync(string txHash);
        Task<IResult<double>> GetTFuelPriceAsync();
    }
}