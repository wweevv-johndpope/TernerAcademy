using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Application.InstructorPortal.CourseSubscriptions.Queries.GetAll;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CourseSubscriptionWebService : WebServiceBase, ICourseSubscriptionWebService
    {
        public CourseSubscriptionWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<GetAllCourseSubscriptionsResponseDto>> GetAllAsync(int courseId, string accessToken) => GetAsync<GetAllCourseSubscriptionsResponseDto>(string.Format(CourseSubscriptionEndpoints.GetAll, courseId), accessToken);
    }
}
