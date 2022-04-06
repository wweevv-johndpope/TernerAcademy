using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICourseReviewManager : IManager
    {
        Task<IResult<GetAllCourseReviewsResponseDto>> GetAllAsync(int courseId);
    }
}