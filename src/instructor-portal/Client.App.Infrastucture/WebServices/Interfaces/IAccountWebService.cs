using Application.Common.Models;
using Application.InstructorPortal.Account.Commands.ChangePassword;
using Application.InstructorPortal.Account.Commands.Login;
using Application.InstructorPortal.Account.Commands.Register;
using Application.InstructorPortal.Account.Commands.UpdateProfile;
using Application.InstructorPortal.Account.Commands.UpdateProfileBio;
using Application.InstructorPortal.Account.Commands.UploadProfilePicture;
using Application.InstructorPortal.Account.Dtos;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IAccountWebService : IWebService
    {
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RegisterAsync(RegisterCommand request);
        Task<IResult> ChangePasswordAsync(ChangePasswordCommand request, string accessToken);
        Task<IResult<InstructorMyProfileDto>> GetProfileAsync(string accessToken);
        Task<IResult> UpdateProfileAsync(UpdateProfileCommand request, string accessToken);
        Task<IResult> UpdateProfileBioAsync(UpdateProfileBioCommand request, string accessToken);
        Task<IResult> UploadProfilePhotoAsync(UploadProfilePictureCommand request, Stream fileStream, string filename, string accessToken);
    }
}