using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.InstructorPortal
{
    public class CourseReviewController : HttpFunctionBase
    {
        public CourseReviewController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_CourseReview_GetAll")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/{courseId}/review")] GetAllCourseReviewsQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetAllCourseReviewsQuery, Result<GetAllCourseReviewsResponseDto>>(context, logger, req, queryArg);
        }
    }
}
