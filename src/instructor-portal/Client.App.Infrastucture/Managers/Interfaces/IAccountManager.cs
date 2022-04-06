using Application.Common.Models;
using Application.InstructorPortal.Account.Commands.ChangePassword;
using Application.InstructorPortal.Account.Commands.Login;
using Application.InstructorPortal.Account.Commands.Register;
using Application.InstructorPortal.Account.Commands.UpdateProfile;
using Application.InstructorPortal.Account.Commands.UpdateProfileBio;
using Application.InstructorPortal.Account.Dtos;
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
        Task<IResult<InstructorMyProfileDto>> GetProfileAsync();
        Task<InstructorMyProfileDto> FetchProfileAsync();
        Task<IResult> UpdateProfileAsync(UpdateProfileCommand request);
        Task<IResult> UpdateProfileBioAsync(UpdateProfileBioCommand request);
        Task<IResult> UploadProfilePhotoAsync(Stream fileStream, string filename);
        Task<IResult> LogoutAsync();
    }
}