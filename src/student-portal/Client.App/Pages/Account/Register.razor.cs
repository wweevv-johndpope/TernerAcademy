using Application.StudentPortal.Account.Commands.Register;
using Blazored.FluentValidation;
using Client.Infrastructure.Exceptions;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Account
{
    public partial class Register
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private RegisterCommand Model { get; set; } = new();
        public bool IsProcessing { get; set; }

        private async Task SubmitAsync()
        {
            if (Validated)
            {
                IsProcessing = true;

                try
                {
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.RegisterAsync(Model));
                    _appDialogService.ShowSuccess(string.Format("Hi {0}, you've successfully registered. Please login to continue.", Model.Name));
                    await Task.Delay(1000);
                    _navigationManager.NavigateTo("/", true);
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