using Application.InstructorPortal.Account.Commands.UpdateProfile;
using Application.InstructorPortal.Account.Commands.UpdateProfileBio;
using Application.InstructorPortal.Account.Dtos;
using Application.InstructorPortal.Communities.Commands.Delete;
using Application.InstructorPortal.Communities.Commands.Update;
using Application.InstructorPortal.Communities.Dtos;
using Client.App.Pages.Account.Modal;
using Client.App.Pages.Communties.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class ProfilePage
    {
        public bool IsProcessing { get; set; }

        public bool IsLoaded { get; set; }

        public InstructorMyProfileDto Profile { get; set; }
        public bool IsProfilePhotoOverlayVisible { get; set; }
        public bool IsUploadingProfilePhoto { get; set; }

        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchProfileAsync();
                await SetStylesAsync(AppBreakpointService.CurrentBreakpoint);
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task FetchProfileAsync()
        {
            Profile = await AccountManager.FetchProfileAsync();
        }

        private void ToggleProfilePhotoOvelay(bool visible)
        {
            if (!IsUploadingProfilePhoto)
            {
                IsProfilePhotoOverlayVisible = visible;
            }
        }

        private async Task SetStylesAsync(Breakpoint e)
        {

        }

        private async Task OnProfilePictureChange(InputFileChangeEventArgs e)
        {
            long maxFileSize = 1024 * 1024 * 10;

            foreach (var file in e.GetMultipleFiles(1))
            {
                var stream = file.OpenReadStream(maxFileSize);
                await UpdateProfilePhotoAsync(file.Name, stream);
            }
        }

        private async Task UpdateProfilePhotoAsync(string filename, Stream stream)
        {
            try
            {
                IsUploadingProfilePhoto = true;
                IsProfilePhotoOverlayVisible = false;
                await InvokeAsync(StateHasChanged);

                await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.UploadProfilePhotoAsync(stream, filename));
                await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.GetProfileAsync());
                await FetchProfileAsync();

                IsUploadingProfilePhoto = false;
                await InvokeAsync(StateHasChanged);
                _appDialogService.ShowSuccess("Profile Photo Updated.");
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
        }

        private async Task InvokeUpdateProfileModalAsync()
        {
            var parameters = new DialogParameters()
            {
                 { nameof(UpdateProfileModal.Model), new UpdateProfileCommand() { Name = Profile.Name, CompanyName = Profile.CompanyName} },
            };

            var dialog = _dialogService.Show<UpdateProfileModal>("Update Profile", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchProfileAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        private void InvokeChangePasswordModal()
        {
            _dialogService.Show<ChangePasswordModal>("Change Password");
        }

        private async Task InvokeCreateCommunityModalAsync()
        {
            var dialog = _dialogService.Show<CreateCommunityModal>("Add Social");
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchProfileAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeUpdateCommunityModalAsync(InstructorCommunityDto community)
        {
            var parameters = new DialogParameters()
            {
                 { nameof(UpdateCommunityModal.Model), new UpdateInstructorCommunityCommand() { CommunityId = community.Id, HandleName = community.HandleName } },
                 { nameof(UpdateCommunityModal.Platform), community.Platform }
            };

            var dialog = _dialogService.Show<UpdateCommunityModal>("Update Social", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchProfileAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task OnInvokeDeleteCommunityModalAsync(InstructorCommunityDto community)
        {
            var parameters = new DialogParameters()
            {
                 { nameof(DeleteCommunityModal.Model), new DeleteInstructorCommunityCommand()
                    {
                        CommunityId = community.Id
                    }
                },
                { nameof(DeleteCommunityModal.DisplayName), $"{community.HandleName} ({community.Platform})" }
            };

            var dialog = _dialogService.Show<DeleteCommunityModal>("Remove Social", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchProfileAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeUpdateProfileBioModalAsync()
        {
            var parameters = new DialogParameters()
            {
                 { nameof(UpdateProfileBioModal.Model), new UpdateProfileBioCommand() { Bio = Profile.Bio ?? "" } },
            };

            var dialog = _dialogService.Show<UpdateProfileBioModal>("Update Profile Bio", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchProfileAsync();
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}