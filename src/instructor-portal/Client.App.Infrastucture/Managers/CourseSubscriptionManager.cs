using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Application.InstructorPortal.CourseSubscriptions.Queries.GetAll;
using Client.App.Infrastructure.WebServices;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CourseSubscriptionManager : ManagerBase, ICourseSubscriptionManager
    {
        private readonly ICourseSubscriptionWebService _courseSubscriptionWebService;
        public CourseSubscriptionManager(IManagerToolkit managerToolkit, ICourseSubscriptionWebService courseSubscriptionWebService) : base(managerToolkit)
        {
            _courseSubscriptionWebService = courseSubscriptionWebService;
        }

        public async Task<IResult<GetAllCourseSubscriptionsResponseDto>> GetAllAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseSubscriptionWebService.GetAllAsync(courseId, AccessToken);
        }
    }
}
