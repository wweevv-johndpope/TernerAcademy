using Application.InstructorPortal.Account.Commands.Login;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Account
{
    public partial class Login
    {
        [Inject] public IWalletManager WalletManager { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private LoginCommand Model { get; set; } = new();
        public bool IsProcessing { get; set; }

        private async Task SubmitAsync()
        {
            if (Validated)
            {
                IsProcessing = true;

                try
                {
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.LoginAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.GetProfileAsync());
                    await _exceptionHandler.HandlerRequestTaskAsync(() => GeneralManager.GetCourseDataAsync());
                    await _exceptionHandler.HandlerRequestTaskAsync(() => GeneralManager.GetTFuelPriceAsync());
                    await _exceptionHandler.HandlerRequestTaskAsync(() => WalletManager.GetWalletInfoAsync());
                    _appDialogService.ShowSuccess("Welcome back");
                    await Task.Delay(1000);
                    _navigationManager.NavigateTo("/", true);
                }
                catch (ApiOkFailedException ex)
                {
                    await AccountManager.LogoutAsync();
                    _appDialogService.ShowErrors(ex.Messages);
                }
                catch (Exception ex)
                {
                    await AccountManager.LogoutAsync();
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