using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CourseReviewWebService : WebServiceBase, ICourseReviewWebService
    {
        public CourseReviewWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<GetAllCourseReviewsResponseDto>> GetAllAsync(int courseId, string accessToken) => GetAsync<GetAllCourseReviewsResponseDto>(string.Format(CourseReviewEndpoints.GetAll, courseId), accessToken);
    }
}
