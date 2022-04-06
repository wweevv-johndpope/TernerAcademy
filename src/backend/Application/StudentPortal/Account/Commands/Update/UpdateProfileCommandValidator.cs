using FluentValidation;

namespace Application.StudentPortal.Account.Commands.Update
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be between 3 and 100 characters long.")
                .MaximumLength(100).WithMessage("Name must be between 3 and 100 characters long.");
        }
    }
}
