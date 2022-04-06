using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Wallets.Commands.Update;
using Application.InstructorPortal.Wallets.Queries.Get;
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
    public class WalletController : HttpFunctionBase
    {
        public WalletController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_Wallet_GetInfo")]
        public async Task<IActionResult> GetInfo([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/wallet")] GetWalletQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetWalletQuery, Result<WalletInfoDto>>(context, logger, req, queryArgs);
        }


        [FunctionName("InstructorPortal_Wallet_Update")]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/wallet")] UpdateWalletCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateWalletCommand, IResult>(context, logger, req, commandArg);
        }
    }
}
