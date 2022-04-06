using Application.Common.Dtos;
using Application.InstructorPortal.Courses.Commands.Create;
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
    public partial class CreateCourseModal : IModalBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Parameter] public CreateCourseCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }
        public string DialogContentContainerStyle { get; set; } = "";

        public List<CourseLanguageDto> CourseLanguages { get; set; } = new();
        public List<CategoryTopicViewItem> CourseCategoryTopics { get; set; } = new();

        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchDataAsync();
                await SetStylesAsync(AppBreakpointService.CurrentBreakpoint);
            }
        }

        public async Task FetchDataAsync()
        {
            CourseLanguages = await GeneralManager.FetchCourseLanguagesAsync();
            Model.LanguageId = CourseLanguages.First(x => x.Name == "English").Id;
            Model.Level = Application.Common.Constants.AppConstants.CourseLevel.First();
            CourseCategoryTopics = await GeneralManager.FetchCourseCategoriesAsync();
            await InvokeAsync(StateHasChanged);
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

        private async Task CreateAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    var courseCreationResponse = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.CreateAsync(Model));
                    _appDialogService.ShowSuccess("New course has been created.");
                    _navigationManager.NavigateTo($"course/{courseCreationResponse.Data}");
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
            return $"{string.Join(", ", selectedValues.Select(x => CourseCategoryTopics.First(y => y.TopicId.ToString() == x).Topic).ToList())}";
        }
    }
}