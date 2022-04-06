using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Wallets.Queries.Get;
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

namespace WebApi.Apis.StudentPortal
{
    public class WalletController : HttpFunctionBase
    {
        public WalletController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StudentPortal_Wallet_GetInfo")]
        public async Task<IActionResult> GetInfo([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/wallet")] GetWalletQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetWalletQuery, Result<WalletInfoDto>>(context, logger, req, queryArgs);
        }
    }
}
