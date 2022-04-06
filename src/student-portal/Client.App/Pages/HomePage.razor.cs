using Application.StudentPortal.Courses.Commands.Search;
using Application.StudentPortal.Courses.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Courses.Modal;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages
{
    public partial class HomePage
    {
        [Inject] public ICourseManager CourseManager { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public List<StudentCourseItemDto> Courses { get; set; } = new();

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
                var result = await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.SearchAsync(new SearchCourseCommand()
                {
                    SearchQuery = ""
                }));
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

        private void InvokeCoursePreviewerModal(StudentCourseItemDto course)
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };
            var parameters = new DialogParameters()
            {
                 { nameof(CoursePreviewerModal.CourseId), course.CourseId },
            };

            _dialogService.Show<CoursePreviewerModal>("Course Preview", parameters, options);
        }
    }
}