using Application.StudentPortal.Account.Commands.ChangePassword;
using Blazored.FluentValidation;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Account.Modal
{
    public partial class ChangePasswordModal : IModalBase
    {
        [Parameter] public ChangePasswordCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task UpdateAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.ChangePasswordAsync(Model));
                    await AccountManager.LogoutAsync();
                    _navigationManager.NavigateTo("/auth/login");
                    _appDialogService.ShowSuccess("You've successfully updated your password.");
                    MudDialog.Close();
                }
                catch (ApiOkFailedException ex)
                {
                    _appDialogService.ShowErrors(ex.Messages);
                }
                catch (Exception ex)
                {
                    _appDialogService.ShowError(ex.Message);
                }

                IsProcessing = false;
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
    }
}