using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Instructors.Dtos;
using Application.StudentPortal.Instructors.Queries.Get;
using Application.StudentPortal.Instructors.Queries.GetCourses;
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
    public class InstructorController : HttpFunctionBase
    {
        public InstructorController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StudentPortal_Instructor_GetDetails")]
        public async Task<IActionResult> GetDetails([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/instructor/{instructorId}/details")] GetInstructorDetailsQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetInstructorDetailsQuery, Result<StudentInstructorDto>>(context, logger, req, queryArgs);
        }

        [FunctionName("StudentPortal_Instructor_GetCourses")]
        public async Task<IActionResult> GetCourses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/instructor/{instructorId}/courses")] GetInstructorListedCoursesQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<GetInstructorListedCoursesQuery, Result<List<StudentInstructorCourseDto>>>(context, logger, req, queryArgs);
        }
    }
}
