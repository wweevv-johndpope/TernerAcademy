using Application.Common.Models;
using Application.InstructorPortal.CourseSubscriptions.Queries.GetAll;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICourseSubscriptionWebService : IWebService
    {
        Task<IResult<GetAllCourseSubscriptionsResponseDto>> GetAllAsync(int courseId, string accessToken);
    }
}