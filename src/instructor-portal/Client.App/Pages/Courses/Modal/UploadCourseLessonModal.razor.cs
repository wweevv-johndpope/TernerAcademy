using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class UploadCourseLessonModal : IModalBase
    {
        [Inject] public ICourseLessonManager CourseLessonManager { get; set; }
        [Parameter] public UploadCourseLessonCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public IBrowserFile SelectedFile { get; set; }
        public string SelectedFileNameDisplay { get; set; }
        private long MaxVideoLimit = 100 * 1024 * 1024;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task UploadAsync()
        {
            if (Validated && !IsProcessing)
            {
                try
                {
                    if (SelectedFile == null)
                    {
                        _appDialogService.ShowError("Video file is required.");
                        return;
                    }

                    IsProcessing = true;
                    var stream = SelectedFile.OpenReadStream(MaxVideoLimit);

                    await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.UploadAsync(Model, stream, SelectedFile.Name));
                    _appDialogService.ShowSuccess("New course lesson has been upload.");
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

        private async Task OnFileChanged(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(1))
            {
                if (file.Size > MaxVideoLimit)
                {
                    _appDialogService.ShowError("Testnet Video Limit: 100 MB");
                    return;
                }

                SelectedFile = file;
                SelectedFileNameDisplay = $"{SelectedFile.Name} ({(SelectedFile.Size / 1024.0 / 1024.0):N2} MB)";
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}