using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICourseReviewWebService : IWebService
    {
        Task<IResult<GetAllCourseReviewsResponseDto>> GetAllAsync(int courseId, string accessToken);
    }
}