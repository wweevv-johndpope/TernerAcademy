using Application.Common.Models;
using Application.InstructorPortal.Account.Commands.ChangePassword;
using Application.InstructorPortal.Account.Commands.Login;
using Application.InstructorPortal.Account.Commands.Register;
using Application.InstructorPortal.Account.Commands.UpdateProfile;
using Application.InstructorPortal.Account.Commands.UpdateProfileBio;
using Application.InstructorPortal.Account.Commands.UploadProfilePicture;
using Application.InstructorPortal.Account.Dtos;
using Client.App.Infrastructure.WebServices;
using Client.Infrastructure.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class AccountManager : ManagerBase, IAccountManager
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly AppRouteViewService _appRouteViewService;
        private readonly IAccountWebService _accountWebService;

        public AccountManager(IManagerToolkit managerToolkit, AppRouteViewService appRouteViewService, AuthenticationStateProvider authenticationStateProvider, IAccountWebService accountWebService)
            : base(managerToolkit)
        {
            _appRouteViewService = appRouteViewService;
            _authenticationStateProvider = authenticationStateProvider;
            _accountWebService = accountWebService;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request)
        {
            var response = await _accountWebService.LoginAsync(request);

            if (response.Succeeded)
            {
                var data = response.Data;
                await ManagerToolkit.SaveAuthTokenHandler(data);
                ((AppStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(data.Token);
                await _appRouteViewService.Populate();
                return await Result<LoginCommandResponse>.SuccessAsync(data);
            }

            return await Result<LoginCommandResponse>.FailAsync(response.Messages);
        }

        public async Task<IResult> RegisterAsync(RegisterCommand request)
        {
            return await _accountWebService.RegisterAsync(request);
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordCommand request)
        {
            await PrepareForWebserviceCall();
            return await _accountWebService.ChangePasswordAsync(request, AccessToken);
        }

        public async Task<IResult<InstructorMyProfileDto>> GetProfileAsync()
        {
            await PrepareForWebserviceCall();
            var response = await _accountWebService.GetProfileAsync(AccessToken);
            if (response.Succeeded)
            {
                await ManagerToolkit.SaveProfile(response.Data);
            }
            return response;
        }

        public Task<InstructorMyProfileDto> FetchProfileAsync() => ManagerToolkit.GetProfile();

        public async Task<IResult> UpdateProfileAsync(UpdateProfileCommand request)
        {
            await PrepareForWebserviceCall();
            return await _accountWebService.UpdateProfileAsync(request, AccessToken);
        }

        public async Task<IResult> UpdateProfileBioAsync(UpdateProfileBioCommand request)
        {
            await PrepareForWebserviceCall();
            return await _accountWebService.UpdateProfileBioAsync(request, AccessToken);
        }

        public async Task<IResult> UploadProfilePhotoAsync(Stream fileStream, string filename)
        {
            await PrepareForWebserviceCall();
            return await _accountWebService.UploadProfilePhotoAsync(new UploadProfilePictureCommand(), fileStream, filename, AccessToken);
        }

        public async Task<IResult> LogoutAsync()
        {
            await ManagerToolkit.ClearAuthTokenHandler();
            ((AppStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            await _appRouteViewService.Populate();
            return await Result.SuccessAsync();
        }
    }
}