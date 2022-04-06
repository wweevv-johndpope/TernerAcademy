using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Communities.Commands.Create;
using Application.InstructorPortal.Communities.Commands.Delete;
using Application.InstructorPortal.Communities.Commands.Update;
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
    public class CommunityController : HttpFunctionBase
    {
        public CommunityController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_Community_Create")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/community")] CreateInstructorCommunityCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<CreateInstructorCommunityCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Community_Update")]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/community/{communityId}")] UpdateInstructorCommunityCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateInstructorCommunityCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Community_Delete")]
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/community/{communityId}/delete")] DeleteInstructorCommunityCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<DeleteInstructorCommunityCommand, IResult>(context, logger, req, commandArg);
        }
    }
}
