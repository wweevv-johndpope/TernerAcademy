using Application.Common.Models;
using Application.InstructorPortal.Dashboard.Queries.Get;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<GetDashboardResponseDto>> GetDashboardAsync();
        Task<GetDashboardResponseDto> FetchDashboardAsync();
    }
}