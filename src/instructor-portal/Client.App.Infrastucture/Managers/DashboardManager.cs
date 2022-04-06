using Application.Common.Models;
using Application.InstructorPortal.Dashboard.Queries.Get;
using Client.App.Infrastructure.WebServices;
using Client.Infrastructure.Constants;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class DashboardManager : ManagerBase, IDashboardManager
    {
        private readonly IDashboardWebService _dashboardWebService;

        public DashboardManager(IManagerToolkit managerToolkit, IDashboardWebService dashboardWebService) : base(managerToolkit)
        {
            _dashboardWebService = dashboardWebService;
        }

        public async Task<IResult<GetDashboardResponseDto>> GetDashboardAsync()
        {
            await PrepareForWebserviceCall();
            var response = await _dashboardWebService.GetDashboardAsync(AccessToken);

            if (response.Succeeded)
            {
                await ManagerToolkit.SaveDataAsync(StorageConstants.Local.Dashboard, response.Data);
            }

            return response;
        }

        public async Task<GetDashboardResponseDto> FetchDashboardAsync()
        {
            var data = await ManagerToolkit.GetDataAsync<GetDashboardResponseDto>(StorageConstants.Local.Dashboard);
            return data ?? new();
        }
    }
}
