using Application.InstructorPortal.Communities.Commands.Delete;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Communties.Modal
{
    public partial class DeleteCommunityModal : IModalBase
    {
        [Inject] public ICommunityManager CommunityManager { get; set; }
        [Parameter] public DeleteInstructorCommunityCommand Model { get; set; } = new();
        [Parameter] public string DisplayName { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task DeleteAsync()
        {
            if (Validated && !IsProcessing)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CommunityManager.DeleteAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.GetProfileAsync());
                    _appDialogService.ShowSuccess($"{DisplayName} has been deleted.");
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