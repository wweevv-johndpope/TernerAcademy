using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseSubscriptions.Queries.GetAll;
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
    public class CourseSubscriptionController : HttpFunctionBase
    {
        public CourseSubscriptionController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_CourseSubscription_GetAll")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/{courseId}/subscription")] GetAllCourseSubscriptionsQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetAllCourseSubscriptionsQuery, Result<GetAllCourseSubscriptionsResponseDto>>(context, logger, req, queryArg);
        }
    }
}
