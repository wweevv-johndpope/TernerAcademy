using Application.InstructorPortal.CourseLessons.Commands.Delete;
using Application.InstructorPortal.CourseLessons.Commands.Update;
using Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering;
using Application.InstructorPortal.CourseLessons.Commands.UploadLesson;
using Application.InstructorPortal.CourseLessons.Dtos;
using Application.InstructorPortal.Courses.Commands.Publish;
using Application.InstructorPortal.Courses.Commands.Update;
using Application.InstructorPortal.Courses.Commands.UploadThumbnail;
using Application.InstructorPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Courses.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses
{
    public partial class CourseDetailsPage : IPageBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Inject] public ICourseLessonManager CourseLessonManager { get; set; }
        [Parameter] public int CourseId { get; set; }
        public bool IsLoaded { get; set; }

        public bool IsThumbnailOverlayVisible { get; set; }
        public bool IsUploadingThumbnail { get; set; }
        public InstructorCourseDto Course { get; set; } = new();
        public List<InstructorCourseLessonDto> Lessons { get; set; } = new();
        public InstructorCourseLessonDto CurrentLesson { get; set; }
        public bool IsFetchingLessons { get; set; }
        public bool IsProcessingOrdering { get; set; }
        public string VideoOverlayStyle { get; set; }
        public bool IsVideoOverlayVisible { get; set; }

        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var isSuccess = await FetchCourseDetailsAsync();

                if (!isSuccess)
                {
                    await Task.Delay(2000);
                    _navigationManager.NavigateTo("/", true);
                    return;
                }

                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
                await SetStylesAsync(AppBreakpointService.CurrentBreakpoint);
            }
        }

        public async Task SetStylesAsync(Breakpoint breakpoint)
        {
            switch (breakpoint)
            {
                case Breakpoint.Xs: VideoOverlayStyle = "height:50vh; width: 300px"; break;
                case Breakpoint.Sm: VideoOverlayStyle = "height:60vh; width: 580px"; break;
                case Breakpoint.Md: VideoOverlayStyle = "height:70vh; width: 940px "; break;
                case Breakpoint.Lg: VideoOverlayStyle = "height:80vh; width: 1260px "; break;
                case Breakpoint.Xl: VideoOverlayStyle = "height:80vh; width: 1900px"; break;
                case Breakpoint.Xxl: VideoOverlayStyle = "height:80vh; width: 2540px"; break;
            }
            await InvokeAsync(StateHasChanged);
        }

        public async Task<bool> FetchCourseDetailsAsync()
        {
            try
            {
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.GetDetailsAsync(CourseId));
                Course = result.Data.Course;
                Lessons = result.Data.Lessons;
                return true;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            return false;
        }

        public async Task FetchCourseLessonsAsync()
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.GetAllAsync(CourseId));
                Lessons = result.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
        }

        private void ToggleThumbnailOvelay(bool visible)
        {
            if (!IsUploadingThumbnail && Course.ListingStatus == Domain.Enums.CourseListingStatus.Draft)
            {
                IsThumbnailOverlayVisible = visible;
            }
        }

        private async Task OnThumbnailChange(InputFileChangeEventArgs e)
        {
            long maxFileSize = 1024 * 1024 * 10;

            foreach (var file in e.GetMultipleFiles(1))
            {
                var stream = file.OpenReadStream(maxFileSize);
                await UpdateThumbnailAsync(file.Name, stream);
            }
        }

        private async Task UpdateThumbnailAsync(string filename, Stream stream)
        {
            try
            {
                IsUploadingThumbnail = true;
                IsThumbnailOverlayVisible = false;
                await InvokeAsync(StateHasChanged);

                await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.UploadThumbnailAsync(new UploadCourseThumbnailCommand() { CourseId = CourseId }, stream, filename));
                await FetchCourseDetailsAsync();

                IsUploadingThumbnail = false;
                await InvokeAsync(StateHasChanged);

                _appDialogService.ShowSuccess("Course Thumbnail Updated.");
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
        }

        private async Task InvokeUpdateCourseModalAsync()
        {
            var topics = await GeneralManager.FetchCourseCategoriesAsync();
            var languages = await GeneralManager.FetchCourseLanguagesAsync();
            var parameters = new DialogParameters()
            {
                 { nameof(UpdateCourseModal.CourseCategoryTopics), topics },
                 { nameof(UpdateCourseModal.CourseLanguages), languages},
                 { nameof(UpdateCourseModal.Model), new UpdateCourseCommand()
                                                    {
                                                        CourseId = Course.Id,
                                                        Name = Course.Name,
                                                        Description= Course.Description,
                                                        ShortDescription = Course.ShortDescription,
                                                        Level = Course.Level,
                                                        LanguageId = Course.LanguageId,
                                                        TopicIds = Course.TopicIds.Split(',').Select(x => Convert.ToInt32(x)),
                                                        PriceInTFuel = Course.PriceInTFuel
                                                    }
                }
            };

            var dialog = _dialogService.Show<UpdateCourseModal>("Update Course", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                IsFetchingLessons = true;
                await FetchCourseDetailsAsync();
                IsFetchingLessons = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeUploadLessonModalAsync()
        {
            var parameters = new DialogParameters()
            {
                 { nameof(UploadCourseLessonModal.Model), new UploadCourseLessonCommand() { CourseId = CourseId, LessonName = "" } },
            };

            var dialog = _dialogService.Show<UploadCourseLessonModal>("Upload New Lesson", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                IsFetchingLessons = true;
                await FetchCourseLessonsAsync();
                IsFetchingLessons = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeUpdateLessonModalAsync(InstructorCourseLessonDto lesson)
        {
            var parameters = new DialogParameters()
            {
                 { nameof(UpdateCourseLessonModal.Model), new UpdateCourseLessonCommand() { CourseId = CourseId, LessonId = lesson.Id } },
            };

            var dialog = _dialogService.Show<UpdateCourseLessonModal>("Update Lesson", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                IsFetchingLessons = true;
                await FetchCourseLessonsAsync();
                IsFetchingLessons = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task InvokeDeleteLessonModalAsync(InstructorCourseLessonDto lesson)
        {
            var parameters = new DialogParameters()
            {
                 { nameof(DeleteCourseLessonModal.Model), new DeleteCourseLessonCommand() { CourseId = CourseId, LessonId = lesson.Id } },
                 { nameof(DeleteCourseLessonModal.LessonName), lesson.Name},
            };

            var dialog = _dialogService.Show<DeleteCourseLessonModal>("Delete Lesson", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                IsFetchingLessons = true;
                await FetchCourseLessonsAsync();
                IsFetchingLessons = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task MoveLessonAsync(InstructorCourseLessonDto lesson, bool isUpward)
        {
            try
            {
                IsProcessingOrdering = true;
                await _exceptionHandler.HandlerRequestTaskAsync(() => CourseLessonManager.UpdateOrderingAsync(new UpdateCourseLessonOrderingCommand() { CourseId = CourseId, LessonId = lesson.Id, IsUpward = isUpward }));
                await FetchCourseLessonsAsync();
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            IsProcessingOrdering = false;
        }

        private void InvokeCourseLessonPreviewOverlay(InstructorCourseLessonDto lesson)
        {
            CurrentLesson = lesson;
            IsVideoOverlayVisible = true;
        }

        private async Task InvokePublishCourseModalAsync()
        {
            var parameters = new DialogParameters()
            {
                 { nameof(PublishCourseModal.Model), new PublishCourseCommand() { CourseId = CourseId } },
                 { nameof(PublishCourseModal.CourseName), Course.Name},
            };

            var dialog = _dialogService.Show<PublishCourseModal>("Publish Course", parameters);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                IsFetchingLessons = true;
                await FetchCourseDetailsAsync();
                await FetchCourseLessonsAsync();
                IsFetchingLessons = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private void InvokeLessonPreviewerModal(InstructorCourseLessonDto lesson)
        {
            var options = new DialogOptions() { CloseButton = true };
            var parameters = new DialogParameters()
            {
                 { nameof(CourseLessonPreviewerModal.CourseId), CourseId },
                 { nameof(CourseLessonPreviewerModal.LessonId), lesson.Id },
            };

            _dialogService.Show<CourseLessonPreviewerModal>("Course Lesson Preview", parameters, options);
        }
    }
}