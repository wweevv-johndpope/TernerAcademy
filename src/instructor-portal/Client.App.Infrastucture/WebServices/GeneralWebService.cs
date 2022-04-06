using Application.Common.Models;
using Application.General.Queries.GetCourseData;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class GeneralWebService : WebServiceBase, IGeneralWebService
    {
        public GeneralWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<GetCourseDataResponseDto>> GetCourseDataAsync() => GetAsync<GetCourseDataResponseDto>(GeneralEndpoints.GetCourseData);
        public Task<IResult<string>> GetTxAsync(string txHash) => GetAsync<string>(string.Format(GeneralEndpoints.GetTx, txHash));
        public Task<IResult<double>> GetTFuelPriceAsync() => GetAsync<double>(GeneralEndpoints.GetTFuelPrice);


    }
}
