using FluentValidation;

namespace Application.InstructorPortal.CourseLessons.Commands.UploadLesson
{
    public class UploadCourseLessonCommandValidator : AbstractValidator<UploadCourseLessonCommand>
    {
        public UploadCourseLessonCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.LessonName)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(5).WithMessage("Name must be between 5 and 100 characters long.")
            .MaximumLength(100).WithMessage("Name must be between 10 and 100 characters long.");
        }
    }
}
