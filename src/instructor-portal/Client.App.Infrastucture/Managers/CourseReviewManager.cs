using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Client.App.Infrastructure.WebServices;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CourseReviewManager : ManagerBase, ICourseReviewManager
    {
        private readonly ICourseReviewWebService _courseReviewWebService;
        public CourseReviewManager(IManagerToolkit managerToolkit, ICourseReviewWebService courseReviewWebService) : base(managerToolkit)
        {
            _courseReviewWebService = courseReviewWebService;
        }

        public async Task<IResult<GetAllCourseReviewsResponseDto>> GetAllAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseReviewWebService.GetAllAsync(courseId, AccessToken);
        }
    }
}
