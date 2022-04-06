using Application.StudentPortal.Courses.Commands.Buy;
using Application.StudentPortal.Courses.Dtos;
using Application.StudentPortal.Instructors.Dtos;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages.Instructors.Modal
{
    public partial class InstructorPreviewerModal
    {
        [Inject] public IInstructorManager InstructorManager { get; set; }
        [Parameter] public int InstructorId { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsLoadedCourses { get; set; }

        public StudentInstructorDto Instructor { get; set; }
        public List<StudentInstructorCourseDto> Courses { get; set; } = new();

        protected override void OnInitialized()
        {
            AppBreakpointService.BreakpointChanged += async (s, e) => await SetStylesAsync(e);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchInstructorDetails();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
                await SetStylesAsync(AppBreakpointService.CurrentBreakpoint);
                await FetchCourses();
            }
        }

        public async Task SetStylesAsync(Breakpoint breakpoint)
        {
            await InvokeAsync(StateHasChanged);
        }

        private async Task FetchInstructorDetails()
        {
            try
            {
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => InstructorManager.GetDetailsAsync(InstructorId));
                Instructor = result.Data;
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

        private async Task FetchCourses()
        {
            try
            {
                IsLoadedCourses = false;
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => InstructorManager.GetCoursesAsync(InstructorId));
                Courses = result.Data;
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
            finally
            {
                IsLoadedCourses = true;
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}