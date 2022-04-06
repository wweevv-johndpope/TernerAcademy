using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Account.Commands.ChangePassword;
using Application.InstructorPortal.Account.Commands.Login;
using Application.InstructorPortal.Account.Commands.Register;
using Application.InstructorPortal.Account.Commands.UpdateProfile;
using Application.InstructorPortal.Account.Commands.UpdateProfileBio;
using Application.InstructorPortal.Account.Commands.UploadProfilePicture;
using Application.InstructorPortal.Account.Dtos;
using Application.InstructorPortal.Account.Queries.MyProfile;
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

namespace WebApi.Apis.InstructorPortal
{
    public class AccountController : HttpFunctionBase
    {
        public AccountController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("InstructorPortal_Account_Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/account/login")] LoginCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<LoginCommand, Result<LoginCommandResponse>>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Account_Register")]
        public async Task<IActionResult> Register([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/account/register")] RegisterCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            return await ExecuteAsync<RegisterCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Account_ChangePassword")]
        public async Task<IActionResult> ChangePassword([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/account/change-password")] ChangePasswordCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<ChangePasswordCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Account_MyProfile")]
        public async Task<IActionResult> GetMyProfile([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "instructor/account/profile")] MyProfileQuery queryArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<MyProfileQuery, Result<InstructorMyProfileDto>>(context, logger, req, queryArgs);
        }

        [FunctionName("InstructorPortal_Account_Profile_Update")]
        public async Task<IActionResult> UpdateProfile([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/account/profile")] UpdateProfileCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateProfileCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Account_Profile_UpdateBio")]
        public async Task<IActionResult> UpdateProfileBio([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/account/profile/bio")] UpdateProfileBioCommand commandArg, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
            return await ExecuteAsync<UpdateProfileBioCommand, IResult>(context, logger, req, commandArg);
        }

        [FunctionName("InstructorPortal_Account_Profile_UploadPhoto")]
        public async Task<IActionResult> UploadPhoto([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "instructor/account/profile/photo")] UploadProfilePictureCommand commandArgs, HttpRequest req, ExecutionContext context, ILogger logger)
        {
            EnsureUserTypeAuthorization(req, UserType.INSTRUCTOR);
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
