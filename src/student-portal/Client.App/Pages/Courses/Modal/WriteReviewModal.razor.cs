using Application.StudentPortal.Account.Commands.ChangePassword;
using Application.StudentPortal.Courses.Commands.WriteReview;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.Courses.Modal
{
    public partial class WriteReviewModal : IModalBase
    {
        [Inject] public ICourseManager CourseManager { get; set; }

        [Parameter] public WriteCourseReviewCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => CourseManager.WriteCourseReviewAsync(Model));
                    _appDialogService.ShowSuccess("You've successfully submitted a course review.");
                    MudDialog.Close();
                }
                catch (ApiOkFailedException ex)
                {
                    _appDialogService.ShowErrors(ex.Messages);
                }
                catch (Exception ex)
                {
                    _appDialogService.ShowError(ex.Message);
                }

                IsProcessing = false;
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }
    }
}