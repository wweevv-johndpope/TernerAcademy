using Application.StudentPortal.Account.Commands.Update;
using Blazored.FluentValidation;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Account.Modal
{
    public partial class UpdateProfileModal : IModalBase
    {
        [Parameter] public UpdateProfileCommand Model { get; set; } = new();
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
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.UpdateProfileAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.GetProfileAsync());
                    _appDialogService.ShowSuccess("You've successfully updated your profile.");
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
    }
}