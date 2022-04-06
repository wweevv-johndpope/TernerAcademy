using Application.Common.Models;
using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Commands.Search;
using Application.StudentPortal.Courses.Commands.WatchLesson;
using Application.StudentPortal.Courses.Commands.WriteReview;
using Application.StudentPortal.Courses.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICourseWebService : IWebService
    {
        Task<IResult<List<StudentCourseItemDto>>> SearchAsync(SearchCourseCommand request, string accessToken);
        Task<IResult<StudentCoursePreviewDto>> GetPreviewAsync(int courseId, string accessToken);
        Task<IResult<StudentCourseDetailsDto>> GetDetailsAsync(int courseId, string accessToken);
        Task<IResult> BuyAsync(BuyCourseCommand request, string accessToken);
        Task<IResult<List<StudentEnrolledCourseItemDto>>> GetEnrolledCoursesAsync(string accessToken);
        Task<IResult> WatchLessonAsync(WatchLessonCommand request, string accessToken);
        Task<IResult> WriteCourseReviewAsync(WriteCourseReviewCommand request, string accessToken);
    }
}