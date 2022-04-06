using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class DeleteCourseLessonModal : IModalBase
    {
        [Inject] public ICourseLessonManager CourseLessonManager { get; set; }
        [Parameter] public DeleteCourseLessonCommand Model { get; set; } = new();
        [Parameter] public string LessonName { get; set; }
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
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.DeleteAsync(Model));
                    _appDialogService.ShowSuccess($"{LessonName} has been deleted.");
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