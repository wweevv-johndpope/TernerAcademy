using Application.Common.Models;
using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Commands.Search;
using Application.StudentPortal.Courses.Commands.WatchLesson;
using Application.StudentPortal.Courses.Commands.WriteReview;
using Application.StudentPortal.Courses.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICourseManager : IManager
    {
        Task<IResult<List<StudentCourseItemDto>>> SearchAsync(SearchCourseCommand request);
        Task<IResult<StudentCoursePreviewDto>> GetPreviewAsync(int courseId);
        Task<IResult<StudentCourseDetailsDto>> GetDetailsAsync(int courseId);
        Task<IResult> BuyAsync(BuyCourseCommand request);
        Task<IResult<List<StudentEnrolledCourseItemDto>>> GetEnrolledCoursesAsync();
        Task<IResult> WatchLessonAsync(WatchLessonCommand request);
        Task<IResult> WriteCourseReviewAsync(WriteCourseReviewCommand request);
    }
}