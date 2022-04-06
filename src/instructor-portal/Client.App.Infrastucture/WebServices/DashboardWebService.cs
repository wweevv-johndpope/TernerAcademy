using Application.Common.Models;
using Application.InstructorPortal.Dashboard.Queries.Get;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class DashboardWebService : WebServiceBase, IDashboardWebService
    {
        public DashboardWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<GetDashboardResponseDto>> GetDashboardAsync(string accessToken) => GetAsync<GetDashboardResponseDto>(DashboardEndpoints.Get, accessToken);
    }
}
