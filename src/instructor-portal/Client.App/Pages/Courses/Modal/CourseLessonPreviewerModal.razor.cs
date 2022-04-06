using Application.InstructorPortal.CourseLessons.Dtos;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class CourseLessonPreviewerModal
    {
        [Inject] public ICourseLessonManager CourseLessonManager { get; set; }
        [Parameter] public int CourseId { get; set; }
        [Parameter] public int LessonId { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public InstructorCourseLessonDto CourseLesson { get; set; }

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
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.GetSingleAsync(CourseId, LessonId));
                CourseLesson = result.Data;
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
    }
}