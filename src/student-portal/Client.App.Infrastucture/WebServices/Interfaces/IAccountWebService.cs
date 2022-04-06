using Application.Common.Models;
using Application.StudentPortal.Account.Commands.ChangePassword;
using Application.StudentPortal.Account.Commands.Login;
using Application.StudentPortal.Account.Commands.Register;
using Application.StudentPortal.Account.Commands.Update;
using Application.StudentPortal.Account.Commands.UploadProfilePicture;
using Application.StudentPortal.Account.Dtos;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IAccountWebService : IWebService
    {
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RegisterAsync(RegisterCommand request);
        Task<IResult> ChangePasswordAsync(ChangePasswordCommand request, string accessToken);
        Task<IResult<StudentMyProfileDto>> GetProfileAsync(string accessToken);
        Task<IResult> UpdateProfileAsync(UpdateProfileCommand request, string accessToken);
        Task<IResult> UploadPhotoAsync(UploadProfilePictureCommand request, Stream fileStream, string filename, string accessToken);
    }
}