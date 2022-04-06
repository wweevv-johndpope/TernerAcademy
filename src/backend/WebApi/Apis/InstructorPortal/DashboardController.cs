using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Dashboard.Queries.Get;
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
    public class DashboardController : HttpFunctionBase
    {
        public DashboardController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_Dashboard_Get")]
        public async Task<IActionResult> GetDashboard([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/dashboard")] GetDashboardQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetDashboardQuery, Result<GetDashboardResponseDto>>(context, logger, req, queryArg);
        }
    }
}
