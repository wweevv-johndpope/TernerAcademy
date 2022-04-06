using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Account.Commands.ChangePassword;
using Application.StudentPortal.Account.Commands.Login;
using Application.StudentPortal.Account.Commands.Register;
using Application.StudentPortal.Account.Commands.Update;
using Application.StudentPortal.Account.Commands.UploadProfilePicture;
using Application.StudentPortal.Account.Dtos;
using Application.StudentPortal.Account.Queries.MyProfile;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis.StudentPortal
{
    public class AccountController : HttpFunctionBase
    {
        public AccountController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("StudentPortal_Account_Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/account/login")] LoginCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<LoginCommand, Result<LoginCommandResponse>>(context, logger, req, commandArg);
        }

        [FunctionName("StudentPortal_Account_Register")]
        public async Task<IActionResult> Register([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/account/register")] RegisterCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<RegisterCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("StudentPortal_Account_ChangePassword")]
        public async Task<IActionResult> ChangePassword([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/account/change-password")] ChangePasswordCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<ChangePasswordCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("StudentPortal_Account_MyProfile")]
        public async Task<IActionResult> GetMyProfile([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "student/account/profile")] MyProfileQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<MyProfileQuery, Result<StudentMyProfileDto>>(context, logger, req, queryArgs);
        }

        [FunctionName("StudentPortal_Account_UpdateProfile")]
        public async Task<IActionResult> UpdateAccount([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/account/profile")] UpdateProfileCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            return await ExecuteAsync<UpdateProfileCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("StudentPortal_Account_Profile_UploadPhoto")]
        public async Task<IActionResult> UploadPhoto([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "student/account/profile/photo")] UploadProfilePictureCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.STUDENT);
            commandArgs ??= new UploadProfilePictureCommand();

            string name = $"AttachedFile";
            var file = req.Form.Files.GetFile(name);

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();
            string extension = Path.GetExtension(filename);

            commandArgs.FileStream = stream;
            commandArgs.FileExtension = extension;

            return await ExecuteAsync<UploadProfilePictureCommand, IResult>(context, logger, req, commandArgs);
        }
    }
}
