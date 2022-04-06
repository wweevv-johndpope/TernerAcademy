using Application.Common.Models;
using Application.InstructorPortal.Account.Commands.ChangePassword;
using Application.InstructorPortal.Account.Commands.Login;
using Application.InstructorPortal.Account.Commands.Register;
using Application.InstructorPortal.Account.Commands.UpdateProfile;
using Application.InstructorPortal.Account.Commands.UpdateProfileBio;
using Application.InstructorPortal.Account.Commands.UploadProfilePicture;
using Application.InstructorPortal.Account.Dtos;
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
        public Task<IResult<InstructorMyProfileDto>> GetProfileAsync(string accessToken) => GetAsync<InstructorMyProfileDto>(AccountEndpoints.Profile, accessToken);
        public Task<IResult> UpdateProfileAsync(UpdateProfileCommand request, string accessToken) => PostAsync(AccountEndpoints.UpdateProfile, request, accessToken);
        public Task<IResult> UpdateProfileBioAsync(UpdateProfileBioCommand request, string accessToken) => PostAsync(AccountEndpoints.UpdateProfileBio, request, accessToken);
        public Task<IResult> UploadProfilePhotoAsync(UploadProfilePictureCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync(AccountEndpoints.UploadProfilePhoto, request, fileStream, filename, accessToken);
    }
}
