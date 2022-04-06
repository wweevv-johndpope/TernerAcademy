using Application.InstructorPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Courses.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class CoursesPage : IPageBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }

        public bool IsLoaded { get; set; }

        public List<InstructorCourseItemDto> Courses { get; set; } = new();

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
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.GetAllAsync());
                Courses = result.Data;
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
                Courses ??= new();
                IsLoaded = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task InvokeCreateCourseModal()
        {
            var dialog = _dialogService.Show<CreateCourseModal>("Create Course");
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await FetchCoursesAsync();
            }
        }

        public void OnCourseCardClicked(InstructorCourseItemDto course)
        {
            _navigationManager.NavigateTo($"course/{course.Id}");
        }
    }
}