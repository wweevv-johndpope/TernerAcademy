using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Categories.Commands.SetPreferences;
using Application.StudentPortal.Categories.Queries.GetPreferences;
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
    public class CategoryController : HttpFunctionBase
    {
        public CategoryController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StudentPortal_CategoryPreference_GetAll")]
        public async Task<IActionResult> GetPreferences([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/category/preferences")] GetCategoryPreferencesQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetCategoryPreferencesQuery, Result<List<CategoryDto>>>(context, logger, req, queryArg);
        }


        [FunctionName("StudentPortal_CategoryPreference_Set")]
        public async Task<IActionResult> SetPreferences([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/category/preferences")] SetCategoryPreferencesCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<SetCategoryPreferencesCommand, IResult>(context, logger, req, commandArg);
        }

    }
}
