using Application.Common.Dtos;
using Application.InstructorPortal.Courses.Commands.Update;
using Application.InstructorPortal.Wallets.Commands.Update;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Domain.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Wallets.Modal
{
    public partial class UpdateWalletModal : IModalBase
    {
        [Inject] public IWalletManager WalletManager { get; set; }
        [Parameter] public UpdateWalletCommand Model { get; set; } = new();
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
                    await _exceptionHandler.HandlerRequestTaskAsync(() => WalletManager.UpdateWalletAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => WalletManager.GetWalletInfoAsync());
                    _appDialogService.ShowSuccess("Your wallet has been updated.");
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