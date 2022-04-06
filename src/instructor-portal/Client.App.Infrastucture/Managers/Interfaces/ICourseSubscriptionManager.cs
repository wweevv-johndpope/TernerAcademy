using Application.Common.Models;
using Application.InstructorPortal.CourseSubscriptions.Queries.GetAll;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICourseSubscriptionManager : IManager
    {
        Task<IResult<GetAllCourseSubscriptionsResponseDto>> GetAllAsync(int courseId);
    }
}