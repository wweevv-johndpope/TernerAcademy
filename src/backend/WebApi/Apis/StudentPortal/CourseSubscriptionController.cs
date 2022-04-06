using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.CourseSubscriptions.Dtos;
using Application.StudentPortal.CourseSubscriptions.Queries.GetPurchaseHistory;
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
    public class CourseSubscriptionController : HttpFunctionBase
    {
        public CourseSubscriptionController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StudentPortal_CourseSubscription_GetPurchaseHistory")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/course/subscription/purchase-history")] GetPurchaseHistoryQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetPurchaseHistoryQuery, Result<List<StudentCourseSubscriptionPurchaseItemDto>>>(context, logger, req, queryArg);
        }
    }
}
