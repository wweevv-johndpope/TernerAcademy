using Application.InstructorPortal.CourseLessons.Commands.Update;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class UpdateCourseLessonModal : IModalBase
    {
        [Inject] public ICourseLessonManager CourseLessonManager { get; set; }
        [Parameter] public UpdateCourseLessonCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchCourseLessonAsync();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task FetchCourseLessonAsync()
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.GetSingleAsync(Model.CourseId, Model.LessonId));
                Model.LessonName = result.Data.Name;
                Model.LessonNotes = result.Data.Notes;
                Model.LessonIsPreviewable = result.Data.IsPreviewable;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
                MudDialog.Close();
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
                MudDialog.Close();
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task UpdateAsync()
        {
            if (Validated && !IsProcessing)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.UpdateAsync(Model));
                    _appDialogService.ShowSuccess("Course lesson has been updated.");
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