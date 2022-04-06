using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Instructors.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class CoursePreviewerModal
    {
        [Inject] public ICourseManager CourseManager { get; set; }
        [Inject] public IWalletManager WalletManager { get; set; }
        [Parameter] public int CourseId { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public bool IsBuying { get; set; }

        public StudentCoursePreviewDto Course { get; set; }
        public StudentCoursePreviewLessonItemDto CurrentLesson { get; set; }
        public string VideoOverlayStyle { get; set; }
        public bool IsVideoOverlayVisible { get; set; }

        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchCourseLessonAsync();
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
                case Breakpoint.Lg: VideoOverlayStyle = "height:80vh; width: 1260px;"; break;
                case Breakpoint.Xl: VideoOverlayStyle = "height:80vh; width: 1900px;"; break;
                case Breakpoint.Xxl: VideoOverlayStyle = "height:80vh; width: 2540px;"; break;
            }
            await InvokeAsync(StateHasChanged);
        }

        private async Task FetchCourseLessonAsync()
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.GetPreviewAsync(CourseId));
                Course = result.Data;
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

        private void InvokeCourseLessonPreviewOverlay(StudentCoursePreviewLessonItemDto lesson)
        {
            CurrentLesson = lesson;
            IsVideoOverlayVisible = true;
        }

        private async Task BuyCourseAsync()
        {
            try
            {
                IsBuying = true;
                await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.BuyAsync(new BuyCourseCommand() { CourseId = CourseId }));
                await _exceptionHandler.HandlerRequestTaskAsync(() => WalletManager.GetWalletInfoAsync());
                await FetchCourseLessonAsync();
                await InvokeAsync(StateHasChanged);
                _appDialogService.ShowSuccess($"You've successfully purchased {Course.Name}.");
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            IsBuying = false;
        }

        public void OnProceedCourse()
        {
            _navigationManager.NavigateTo($"course/{CourseId}");
        }

        private void InvokeInstructorPreviewerModal()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
            var parameters = new DialogParameters()
            {
                 { nameof(InstructorPreviewerModal.InstructorId), Course.InstructorId},
            };

            MudDialog.Close();
            _dialogService.Show<InstructorPreviewerModal>("About the Instructor", parameters, options);
        }
    }
}