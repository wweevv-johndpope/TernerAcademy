using Application.StudentPortal.Categories.Commands.SetPreferences;
using Blazored.FluentValidation;
using Client.Infrastructure.Exceptions;
using Domain.Views;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Category.Modals
{
    public partial class SetCategoryPreferencesModal : IModalBase
    {
        [Parameter] public SetCategoryPreferencesCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public List<CategoryTopicViewItem> Categories { get; set; } = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SetAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CategoryManager.SetPreferencesAsync(Model));
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CategoryManager.GetPreferencesAsync());
                    _appDialogService.ShowSuccess("Interest Updated.");
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

        protected override async Task OnInitializedAsync()
        {
            var categories = await GeneralManager.FetchCourseCategoriesAsync();
            Categories = categories.GroupBy(x => x.CategoryId).Select(g => g.First()).ToList();
        }

        private void OnSelectedChipsChanged(MudChip[] chips)
        {
            Model.CategoryIds = chips.Select(x => ((CategoryTopicViewItem)x.Tag).CategoryId).ToList();
        }
    }
}