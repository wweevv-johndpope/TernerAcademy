using Application.Common.Dtos;
using Application.StudentPortal.Account.Commands.ChangePassword;
using Application.StudentPortal.Account.Commands.Update;
using Application.StudentPortal.Account.Dtos;
using Client.App.Pages.Account.Modal;
using Client.App.Pages.Category.Modals;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class ProfilePage
    {
        public bool IsProcessing { get; set; }

        public bool IsLoaded { get; set; }

        public StudentMyProfileDto Profile { get; set; }
        public bool IsProfilePhotoOverlayVisible { get; set; }
        public bool IsUploadingProfilePhoto { get; set; }
        public List<CategoryDto> CategoryPreferences { get; set; }

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
            CategoryPreferences = await CategoryManager.FetchPreferencesAsync();
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

                await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.UploadPhotoAsync(stream, filename));
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
                 { nameof(UpdateProfileModal.Model), new UpdateProfileCommand() { Name = Profile.Name } },
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

        private async Task InvokeCategoryPreferencesAsync()
        {
            var dialog = _dialogService.Show<SetCategoryPreferencesModal>("Update Interest");
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await FetchProfileAsync();
            }
        }
    }
}