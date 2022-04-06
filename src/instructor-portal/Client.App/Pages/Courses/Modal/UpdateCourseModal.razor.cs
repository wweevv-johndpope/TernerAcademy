using Application.Common.Dtos;
using Application.InstructorPortal.Courses.Commands.Update;
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

namespace Client.App.Pages.Courses.Modal
{
    public partial class UpdateCourseModal : IModalBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Parameter] public UpdateCourseCommand Model { get; set; }
        [Parameter] public List<CategoryTopicViewItem> CourseCategoryTopics { get; set; }
        [Parameter] public List<CourseLanguageDto> CourseLanguages { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }
        public string DialogContentContainerStyle { get; set; } = "";


        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected async override Task OnInitializedAsync()
        {
            await SetStylesAsync(AppBreakpointService.CurrentBreakpoint);
        }

        public async Task SetStylesAsync(Breakpoint breakpoint)
        {
            if (breakpoint == Breakpoint.Xs)
            {
                DialogContentContainerStyle = "max-height:450px; overflow-y: scroll";
            }
            else
            {
                DialogContentContainerStyle = "max-height:600px; overflow-y: scroll";
            }

            await InvokeAsync(StateHasChanged);
        }

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
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.UpdateAsync(Model));
                    _appDialogService.ShowSuccess("Course has been updated.");
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

        private string GetMultiSelectionTopicText(List<string> selectedValues)
        {
            Console.WriteLine($"Multi: {string.Join(",", CourseCategoryTopics.Select(x => x.TopicId))}");

            if (CourseCategoryTopics.Any())
            {
                return $"{string.Join(", ", selectedValues.Select(x => CourseCategoryTopics.First(y => y.TopicId.ToString() == x).Topic).ToList())}";
            }

            return string.Empty;
        }
    }
}