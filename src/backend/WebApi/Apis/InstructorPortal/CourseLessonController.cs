using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Application.InstructorPortal.CourseLessons.Commands.Update;
using Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering;
using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Application.InstructorPortal.CourseLessons.Dtos;
using Application.InstructorPortal.CourseLessons.Queries.Get;
using Application.InstructorPortal.CourseLessons.Queries.GetAll;
using Application.InstructorPortal.CourseLessons.Queries.GetProcessingCount;
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
    public class CourseLessonController : HttpFunctionBase
    {
        public CourseLessonController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_CourseLesson_Get")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/{courseId}/lesson/{lessonId}")] GetCourseLessonQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetCourseLessonQuery, Result<InstructorCourseLessonDto>>(context, logger, req, queryArg);
        }

        [FunctionName("InstructorPortal_CourseLesson_GetAll")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/{courseId}/lesson")] GetAllCourseLessonsQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetAllCourseLessonsQuery, Result<List<InstructorCourseLessonDto>>>(context, logger, req, queryArg);
        }

        [FunctionName("InstructorPortal_CourseLesson_Upload")]
        public async Task<IActionResult> Upload([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/lesson")] UploadCourseLessonCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);

            commandArgs ??= new UploadCourseLessonCommand();

            var jsonData = JsonConvert.DeserializeObject<UploadCourseLessonCommand>(req.Form["JsonPayload"].ToString());
            commandArgs.CourseId = jsonData.CourseId;
            commandArgs.LessonName = jsonData.LessonName;
            commandArgs.LessonNotes = jsonData.LessonNotes;
            commandArgs.LessonIsPreviewable = jsonData.LessonIsPreviewable;

            string name = $"AttachedFile";
            var file = req.Form.Files.GetFile(name);

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            string extension = Path.GetExtension(filename);

            commandArgs.FileStream = stream;
            commandArgs.FileExtension = extension;

            return await ExecuteAsync<UploadCourseLessonCommand, IResult>(context, logger, req, commandArgs);
        }

        [FunctionName("InstructorPortal_CourseLesson_Update")]
        public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/lesson/{lessonId}")] UpdateCourseLessonCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateCourseLessonCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_CourseLesson_UpdateOrdering")]
        public async Task<IActionResult> UpdateOrdering([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/{courseId}/lesson/order")] UpdateCourseLessonOrderingCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateCourseLessonOrderingCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_CourseLesson_Delete")]
        public async Task<IActionResult> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/course/{courseId}/lesson/{lessonId}/delete")] DeleteCourseLessonCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<DeleteCourseLessonCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_CourseLesson_GetProcessingCount")]
        public async Task<IActionResult> GetProcessingCount([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/course/{courseId}/lesson/processing")] GetCourseLessonProcessingCountQuery queryArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<GetCourseLessonProcessingCountQuery, Result<int>>(context, logger, req, queryArg);
        }
    }
}
