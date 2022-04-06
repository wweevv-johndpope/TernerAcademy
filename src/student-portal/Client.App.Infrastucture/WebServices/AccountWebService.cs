using Application.Common.Models;
using Application.StudentPortal.Account.Commands.ChangePassword;
using Application.StudentPortal.Account.Commands.Login;
using Application.StudentPortal.Account.Commands.Register;
using Application.StudentPortal.Account.Commands.Update;
using Application.StudentPortal.Account.Commands.UploadProfilePicture;
using Application.StudentPortal.Account.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class AccountWebService : WebServiceBase, IAccountWebService
    {
        public AccountWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request) => PostAsync<LoginCommand, LoginCommandResponse>(AccountEndpoints.Login, request);
        public Task<IResult> RegisterAsync(RegisterCommand request) => PostAsync(AccountEndpoints.Register, request);
        public Task<IResult> ChangePasswordAsync(ChangePasswordCommand request, string accessToken) => PostAsync(AccountEndpoints.ChangePassword, request, accessToken);
        public Task<IResult> UpdateProfileAsync(UpdateProfileCommand request, string accessToken) => PostAsync(AccountEndpoints.UpdateProfile, request, accessToken);
        public Task<IResult<StudentMyProfileDto>> GetProfileAsync(string accessToken) => GetAsync<StudentMyProfileDto>(AccountEndpoints.Profile, accessToken);
        public Task<IResult> UploadPhotoAsync(UploadProfilePictureCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync(AccountEndpoints.UploadPhoto, request, fileStream, filename, accessToken);

    }
}
