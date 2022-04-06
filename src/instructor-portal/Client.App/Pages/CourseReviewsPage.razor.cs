using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Application.InstructorPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class CourseReviewsPage : IPageBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Inject] public ICourseReviewManager CourseReviewManager { get; set; }

        public bool IsLoaded { get; set; }
        public bool IsFetchingCourseReview { get; set; }

        public List<InstructorListedCourseDto> ListedCourses { get; set; } = new();
        public GetAllCourseReviewsResponseDto CourseReview { get; set; }
        public int SelectedCourseId { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchCoursesAsync();
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task FetchCoursesAsync()
        {
            try
            {
                IsLoaded = false;
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.GetAllListedAsync());
                var listedCourses = result.Data;
                if (listedCourses.Any())
                {
                    var course = listedCourses.First();
                    var courseReviewResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseReviewManager.GetAllAsync(course.Id));
                    CourseReview = courseReviewResult.Data;
                    SelectedCourseId = course.Id;
                }
                ListedCourses = listedCourses;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
            finally
            {
                ListedCourses ??= new();
                CourseReview ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task FetchCourseReviewsAsync(int courseId)
        {
            try
            {
                IsFetchingCourseReview = true;
                var courseReviewResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseReviewManager.GetAllAsync(courseId));
                CourseReview = courseReviewResult.Data;
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
            finally
            {
                CourseReview ??= new();
                IsFetchingCourseReview = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void OnCourseValueChanged(int courseId)
        {
            SelectedCourseId = courseId;
            await FetchCourseReviewsAsync(courseId);
        }
    }
}