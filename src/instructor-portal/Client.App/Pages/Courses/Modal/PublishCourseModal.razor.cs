using Application.InstructorPortal.Courses.Commands.Publish;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class PublishCourseModal : IModalBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Parameter] public PublishCourseCommand Model { get; set; } = new();
        [Parameter] public string CourseName { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task PublishAsync()
        {
            if (Validated && !IsProcessing)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.PublishAsync(Model));
                    _appDialogService.ShowSuccess($"{CourseName} has been published and listed.");
                    MudDialog.Close();
                }
                catch (ApiOkFailedException ex)
                {
                    _appDialogService.ShowErrors(ex.Messages);
                    MudDialog.Cancel();
                }
                catch (Exception ex)
                {
                    _appDialogService.ShowError(ex.Message);
                    MudDialog.Cancel();
                }

                IsProcessing = false;
            }
        }


    }
}