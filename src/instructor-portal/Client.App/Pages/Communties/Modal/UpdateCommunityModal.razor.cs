using Application.InstructorPortal.Communities.Commands.Update;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Communties.Modal
{
    public partial class UpdateCommunityModal : IModalBase
    {
        [Inject] public ICommunityManager CommunityManager { get; set; }
        [Parameter] public UpdateInstructorCommunityCommand Model { get; set; } = new();
        [Parameter] public string Platform { get; set; }
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
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CommunityManager.UpdateAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.GetProfileAsync());
                    _appDialogService.ShowSuccess("Community has been updated.");
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