using Application.InstructorPortal.CourseReviews.Queries.GetAll;
using Application.InstructorPortal.Courses.Dtos;
using Application.InstructorPortal.CourseSubscriptions.Queries.GetAll;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Transactions.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class CourseSubscriptionsPage : IPageBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Inject] public ICourseSubscriptionManager CourseSubscriptionManager { get; set; }

        public bool IsLoaded { get; set; }
        public bool IsFetchingCourseSubscription { get; set; }

        public List<InstructorListedCourseDto> ListedCourses { get; set; } = new();
        public GetAllCourseSubscriptionsResponseDto CourseSubscription { get; set; }
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
                    var courseSubsResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseSubscriptionManager.GetAllAsync(course.Id));
                    CourseSubscription = courseSubsResult.Data;
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
                CourseSubscription ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task FetchCourseSubscriptionsAsync(int courseId)
        {
            try
            {
                IsFetchingCourseSubscription = true;
                var courseSubsResult = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseSubscriptionManager.GetAllAsync(courseId));
                CourseSubscription = courseSubsResult.Data;
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
                CourseSubscription ??= new();
                IsFetchingCourseSubscription = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void OnCourseValueChanged(int courseId)
        {
            SelectedCourseId = courseId;
            await FetchCourseSubscriptionsAsync(courseId);
        }

        private void InvokeTransactionPreviewerModal(string txHash)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
            var parameters = new DialogParameters()
            {
                 { nameof(TransactionPreviewerModal.TxHash), txHash},
            };

            _dialogService.Show<TransactionPreviewerModal>("Transaction Details", parameters, options);
        }
    }
}