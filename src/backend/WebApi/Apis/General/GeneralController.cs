using Application.Common.Interfaces;
using Application.Common.Models;
using Application.General.Queries.GetCourseData;
using Application.General.Queries.GetTFuelPrice;
using Application.General.Queries.GetTx;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.General
{
    public class GeneralController : HttpFunctionBase
    {
        public GeneralController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("General_GetCourseData")]
        public async Task<IActionResult> GetCourseData([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "general/coursedata")] GetCourseDataQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<GetCourseDataQuery, Result<GetCourseDataResponseDto>>(context, logger, req, queryArgs);
        }

        [FunctionName("General_GetTx")]
        public async Task<IActionResult> GetTx([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "general/transaction/{txHash}")] GetTxQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<GetTxQuery, Result<string>>(context, logger, req, queryArgs);
        }

        [FunctionName("General_GetTFuelPrice")]
        public async Task<IActionResult> GetTFuelPrice([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "general/price/tfuel")] GetTFuelPriceQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<GetTFuelPriceQuery, Result<double>>(context, logger, req, queryArgs);
        }
    }
}
