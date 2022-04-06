using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Commands.Search;
using Application.StudentPortal.Courses.Commands.WatchLesson;
using Application.StudentPortal.Courses.Commands.WriteReview;
using Application.StudentPortal.Courses.Dtos;
using Application.StudentPortal.Courses.Queries.GetCourseDetails;
using Application.StudentPortal.Courses.Queries.GetCoursePreview;
using Application.StudentPortal.Courses.Queries.GetEnrolled;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.StudentPortal
{
    public class CourseController : HttpFunctionBase
    {
        public CourseController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StudentPortal_Course_Search")]
        public async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/course/search")] SearchCourseCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<SearchCourseCommand, Result<List<StudentCourseItemDto>>>(context, logger, req, commandArgs);
        }

        [FunctionName("StudentPortal_Course_GetEnrolledCourses")]
        public async Task<IActionResult> GetEnrolledCourses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/course/enrolled")] GetEnrolledCoursesQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetEnrolledCoursesQuery, Result<List<StudentEnrolledCourseItemDto>>>(context, logger, req, queryArgs);
        }

        [FunctionName("StudentPortal_Course_GetPreview")]
        public async Task<IActionResult> GetPreview([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/course/{courseId}/preview")] GetCoursePreviewQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetCoursePreviewQuery, Result<StudentCoursePreviewDto>>(context, logger, req, queryArgs);
        }

        [FunctionName("StudentPortal_Course_GetDetails")]
        public async Task<IActionResult> GetDetails([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/course/{courseId}/details")] GetCourseDetailsQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetCourseDetailsQuery, Result<StudentCourseDetailsDto>>(context, logger, req, queryArgs);
        }

        [FunctionName("StudentPortal_Course_Buy")]
        public async Task<IActionResult> Buy([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/course/{courseId}/buy")] BuyCourseCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<BuyCourseCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("StudentPortal_Course_Lesson_Watch")]
        public async Task<IActionResult> WatchLesson([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/course/{courseId}/watch")] WatchLessonCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<WatchLessonCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("StudentPortal_Course_WriteReview")]
        public async Task<IActionResult> WriteReview([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/course/{courseId}/review")] WriteCourseReviewCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<WriteCourseReviewCommand, IResult>(context, logger, req, commandArgs);
        }
    }
}
