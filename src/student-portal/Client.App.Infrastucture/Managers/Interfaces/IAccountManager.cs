using Application.Common.Models;
using Application.StudentPortal.Account.Commands.ChangePassword;
using Application.StudentPortal.Account.Commands.Login;
using Application.StudentPortal.Account.Commands.Register;
using Application.StudentPortal.Account.Commands.Update;
using Application.StudentPortal.Account.Dtos;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IAccountManager : IManager
    {
        Task<ClaimsPrincipal> CurrentUser();
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RegisterAsync(RegisterCommand request);
        Task<IResult> ChangePasswordAsync(ChangePasswordCommand request);
        Task<IResult<StudentMyProfileDto>> GetProfileAsync();
        Task<StudentMyProfileDto> FetchProfileAsync();
        Task<IResult> UpdateProfileAsync(UpdateProfileCommand request);
        Task<IResult> UploadPhotoAsync(Stream fileStream, string filename);
        Task<IResult> LogoutAsync();
    }
}