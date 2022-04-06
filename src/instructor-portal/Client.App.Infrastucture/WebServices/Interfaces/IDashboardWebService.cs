using Application.Common.Models;
using Application.InstructorPortal.Dashboard.Queries.Get;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IDashboardWebService : IWebService
    {
        Task<IResult<GetDashboardResponseDto>> GetDashboardAsync(string accessToken);
    }
}