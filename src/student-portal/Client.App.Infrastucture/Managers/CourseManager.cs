using Application.Common.Models;
using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Commands.Search;
using Application.StudentPortal.Courses.Commands.WatchLesson;
using Application.StudentPortal.Courses.Commands.WriteReview;
using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CourseManager : ManagerBase, ICourseManager
    {
        private readonly ICourseWebService _courseWebService;
        public CourseManager(IManagerToolkit managerToolkit, ICourseWebService courseWebService) : base(managerToolkit)
        {
            _courseWebService = courseWebService;
        }

        public async Task<IResult<List<StudentCourseItemDto>>> SearchAsync(SearchCourseCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.SearchAsync(request, AccessToken);
        }

        public async Task<IResult<StudentCoursePreviewDto>> GetPreviewAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetPreviewAsync(courseId, AccessToken);
        }

        public async Task<IResult<StudentCourseDetailsDto>> GetDetailsAsync(int courseId)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetDetailsAsync(courseId, AccessToken);
        }

        public async Task<IResult> BuyAsync(BuyCourseCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.BuyAsync(request, AccessToken);
        }

        public async Task<IResult<List<StudentEnrolledCourseItemDto>>> GetEnrolledCoursesAsync()
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetEnrolledCoursesAsync(AccessToken);
        }

        public async Task<IResult> WatchLessonAsync(WatchLessonCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.WatchLessonAsync(request, AccessToken);
        }

        public async Task<IResult> WriteCourseReviewAsync(WriteCourseReviewCommand request)
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.WriteCourseReviewAsync(request, AccessToken);
        }
    }
}
