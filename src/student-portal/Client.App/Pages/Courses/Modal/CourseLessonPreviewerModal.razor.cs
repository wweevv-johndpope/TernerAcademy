using Application.StudentPortal.Courses.Commands.WatchLesson;
using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class CourseLessonPreviewerModal
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Parameter] public int CourseId { get; set; }
        [Parameter] public StudentCourseDetailsLessonItemDto Lesson { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetWatchLessonAsync();
            }
        }

        private async Task SetWatchLessonAsync()
        {
            try
            {
                await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.WatchLessonAsync(new WatchLessonCommand() { LessonId = Lesson.Id, CourseId = CourseId }));
            }
            catch (Exception)
            {
                //Do Nothing
            }
        }
    }
}