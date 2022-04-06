using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Courses.Commands.Create;
using Application.InstructorPortal.Courses.Commands.Publish;
using Application.InstructorPortal.Courses.Commands.Update;
using Application.InstructorPortal.Courses.Commands.UploadThumbnail;
using Application.InstructorPortal.Courses.Dtos;
using Application.InstructorPortal.Courses.Queries.GetAll;
using Application.InstructorPortal.Courses.Queries.GetAllListed;
using Application.InstructorPortal.Courses.Queries.GetDetails;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.InstructorPortal
{
    public class CourseController : HttpFunctionBase
    {
        public CourseController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_Course_GetAll")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course")] GetAllCoursesQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetAllCoursesQuery, Result<List<InstructorCourseItemDto>>>(context, logger, req, queryArg);
        }

        [FunctionName("InstructorPortal_Course_GetAllListed")]
        public async Task<IActionResult> GetAllListed([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/listed")] GetAllListedCoursesQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetAllListedCoursesQuery, Result<List<InstructorListedCourseDto>>>(context, logger, req, queryArg);
        }

        [FunctionName("InstructorPortal_Course_GetDetails")]
        public async Task<IActionResult> GetDetails([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/{courseId}")] GetCourseDetailsQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetCourseDetailsQuery, Result<CourseDetailsResponseDto>>(context, logger, req, queryArg);
        }


        [FunctionName("InstructorPortal_Course_Create")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course")] CreateCourseCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<CreateCourseCommand, Result<int>>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Course_Update")]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/{courseId}")] UpdateCourseCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateCourseCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Course_UploadThumbnail")]
        public async Task<IActionResult> UploadThumbnail([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/{courseId}/thumbnail")] UploadCourseThumbnailCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);

            commandArgs ??= new UploadCourseThumbnailCommand();

            var jsonData = JsonConvert.DeserializeObject<UploadCourseThumbnailCommand>(req.Form["JsonPayload"].ToString());
            commandArgs.CourseId = jsonData.CourseId;

            string name = $"AttachedFile";
            var file = req.Form.Files.GetFile(name);

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            string extension = Path.GetExtension(filename);

            commandArgs.FileStream = stream;
            commandArgs.FileExtension = extension;

            return await ExecuteAsync<UploadCourseThumbnailCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("InstructorPortal_Course_Publish")]
        public async Task<IActionResult> Publish([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/{courseId}/publish")] PublishCourseCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<PublishCourseCommand, IResult>(context, logger, req, commandArg);
        }


    }
}
