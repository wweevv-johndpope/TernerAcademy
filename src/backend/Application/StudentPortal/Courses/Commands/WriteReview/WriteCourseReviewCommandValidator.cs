using FluentValidation;

namespace Application.StudentPortal.Courses.Commands.WriteReview
{
    public class WriteCourseReviewCommandValidator : AbstractValidator<WriteCourseReviewCommand>
    {
        public WriteCourseReviewCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Rating)
                .GreaterThan(0).WithMessage("Rating must be ranging from 1-5.")
                .LessThan(6).WithMessage("Rating must be ranging from 1-5.");

            RuleFor(v => v.Comment)
                .NotNull().WithMessage("Comment is required.")
                .NotEmpty().WithMessage("Comment is required.")
                .MaximumLength(512).WithMessage("Comment must be less than 512 characters long.");
        }
    }
}
