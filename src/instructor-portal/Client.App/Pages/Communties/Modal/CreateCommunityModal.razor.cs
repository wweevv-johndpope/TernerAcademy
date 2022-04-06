using Application.InstructorPortal.Communities.Commands.Create;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Communties.Modal
{
    public partial class CreateCommunityModal : IModalBase
    {
        [Inject] public ICommunityManager CommunityManager { get; set; }
        [Parameter] public CreateInstructorCommunityCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchDataAsync();
            }
        }

        public async Task FetchDataAsync()
        {
            Model.Platform = Application.Common.Constants.AppConstants.CommunityPlatforms.First();
            await InvokeAsync(StateHasChanged);
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task CreateAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CommunityManager.CreateAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => AccountManager.GetProfileAsync());
                    _appDialogService.ShowSuccess("New community has been created.");
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