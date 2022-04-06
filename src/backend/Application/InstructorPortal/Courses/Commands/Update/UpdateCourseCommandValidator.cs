using FluentValidation;

namespace Application.InstructorPortal.Courses.Commands.Update
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(10).WithMessage("Name must be between 10 and 100 characters long.")
                .MaximumLength(100).WithMessage("Name must be between 10 and 100 characters long.");

            RuleFor(v => v.ShortDescription)
                .NotNull().WithMessage("Short Description is required.")
                .NotEmpty().WithMessage("Short Description is required.")
                .MinimumLength(10).WithMessage("Short Description must be between 10 and 100 characters long.")
                .MaximumLength(100).WithMessage("Short Description must be between 10 and 100 characters long.");

            RuleFor(v => v.Description)
                .NotNull().WithMessage("Description is required.")
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(20).WithMessage("Description must be between 20 and 2048 characters long.")
                .MaximumLength(2048).WithMessage("Description must be between 20 and 2048 characters long.");

            RuleFor(v => v.PriceInTFuel)
                .GreaterThanOrEqualTo(10).WithMessage("[TESTNET/PRIVATENET] Price (TFuel) must be between 10 to 1000 TFuel.")
                .LessThanOrEqualTo(1000).WithMessage("[TESTNET/PRIVATENET] Price (TFuel) must be between 10 to 1000 TFuel.");
        }
    }
}
