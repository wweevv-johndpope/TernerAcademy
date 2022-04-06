using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class MyCoursesPage
    {
        [Inject] public ICourseManager CourseManager { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public List<StudentEnrolledCourseItemDto> Courses { get; set; } = new();

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchCoursesAsync();
            }
        }

        public async Task FetchCoursesAsync()
        {
            try
            {
                IsLoaded = false;
                await InvokeAsync(StateHasChanged);
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.GetEnrolledCoursesAsync());
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

        public void OnCourseCardClicked(StudentEnrolledCourseItemDto course)
        {
            _navigationManager.NavigateTo($"course/{course.CourseId}");
        }
    }
}