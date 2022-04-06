using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Seed.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.Seed
{
    public class SeedController : HttpFunctionBase
    {
        public SeedController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("Seed_CategoriesAndTopics")]
        public async Task<IActionResult> SeedCategories([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "seed/categoriesAndTopics")] SeedCategoryTopicCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<SeedCategoryTopicCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("Seed_CourseLanguages")]
        public async Task<IActionResult> SeedCourseLanguages([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "seed/courseLanguages")] SeedCourseLanguageCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<SeedCourseLanguageCommand, IResult>(context, logger, req, commandArg);
        }
    }
}
