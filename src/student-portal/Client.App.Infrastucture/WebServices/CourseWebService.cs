using Application.Common.Models;
using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Commands.Search;
using Application.StudentPortal.Courses.Commands.WatchLesson;
using Application.StudentPortal.Courses.Commands.WriteReview;
using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CourseWebService : WebServiceBase, ICourseWebService
    {
        public CourseWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<StudentCourseItemDto>>> SearchAsync(SearchCourseCommand request, string accessToken) => PostAsync<SearchCourseCommand, List<StudentCourseItemDto>>(CourseEndpoints.Search, request, accessToken);
        public Task<IResult<StudentCoursePreviewDto>> GetPreviewAsync(int courseId, string accessToken) => GetAsync<StudentCoursePreviewDto>(string.Format(CourseEndpoints.GetPreview, courseId), accessToken);
        public Task<IResult<StudentCourseDetailsDto>> GetDetailsAsync(int courseId, string accessToken) => GetAsync<StudentCourseDetailsDto>(string.Format(CourseEndpoints.GetDetails, courseId), accessToken);
        public Task<IResult> BuyAsync(BuyCourseCommand request, string accessToken) => PostAsync(string.Format(CourseEndpoints.Buy, request.CourseId), request, accessToken);
        public Task<IResult<List<StudentEnrolledCourseItemDto>>> GetEnrolledCoursesAsync(string accessToken) => GetAsync<List<StudentEnrolledCourseItemDto>>(CourseEndpoints.GetEnrolledCourses, accessToken);
        public Task<IResult> WatchLessonAsync(WatchLessonCommand request, string accessToken) => PostAsync(string.Format(CourseEndpoints.WatchLesson, request.CourseId), request, accessToken);
        public Task<IResult> WriteCourseReviewAsync(WriteCourseReviewCommand request, string accessToken) => PostAsync(string.Format(CourseEndpoints.WriteCourseReview, request.CourseId), request, accessToken);

    }
}
