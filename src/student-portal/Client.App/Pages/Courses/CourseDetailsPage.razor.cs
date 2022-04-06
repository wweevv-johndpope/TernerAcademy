using Application.StudentPortal.Courses.Commands.WriteReview;
using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Courses.Modal;
using Client.App.Pages.Instructors.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses
{
    public partial class CourseDetailsPage
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Parameter] public int CourseId { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public List<BreadcrumbItem> BreadcrumbItems { get; set; } = new();
        public StudentCourseDetailsDto Course { get; set; } = new();

        protected override void OnInitialized()
        {
            //AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var isSuccess = await FetchCourseDetailsAsync();

                if (!isSuccess)
                {
                    await Task.Delay(2000);
                    _navigationManager.NavigateTo("/courses", true);
                    return;
                }

                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task<bool> FetchCourseDetailsAsync()
        {
            try
            {
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.GetDetailsAsync(CourseId));
                Course = result.Data;
                BreadcrumbItems = new List<BreadcrumbItem>()
                {
                      new BreadcrumbItem("Courses", href: "/courses", icon: Icons.Material.Filled.Book),
                      new BreadcrumbItem($"{Course.Name}", href: $"/course/{Course.Id}"),
                };

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

        public void InvokeCourseLessonPreviewerModal(StudentCourseDetailsLessonItemDto lesson)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
            var parameters = new DialogParameters()
            {
                 { nameof(CourseLessonPreviewerModal.CourseId), CourseId },
                 { nameof(CourseLessonPreviewerModal.Lesson), lesson },
            };

            _dialogService.Show<CourseLessonPreviewerModal>($"{lesson.Name}", parameters, options);
        }

        public async Task InvokeWriteCourseReviewModalAsync()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };
            var parameters = new DialogParameters()
            {
                 { nameof(WriteReviewModal.Model), new WriteCourseReviewCommand() {
                     CourseId = CourseId,
                     Rating = Course.MyCourseSubscriptionRating.HasValue ? Course.MyCourseSubscriptionRating.Value : 5,
                     Comment = Course.MyCourseSubscriptionComment ?? string.Empty
                 }},
            };

            var dialog = _dialogService.Show<WriteReviewModal>($"Write Review for {Course.Name}", parameters, options);
            var dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                await FetchCourseDetailsAsync();
            }
        }

        private void InvokeInstructorPreviewerModal()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
            var parameters = new DialogParameters()
            {
                 { nameof(InstructorPreviewerModal.InstructorId), Course.InstructorId},
            };

            _dialogService.Show<InstructorPreviewerModal>("About the Instructor", parameters, options);
        }
    }
}