using FluentValidation;

namespace Application.InstructorPortal.Communities.Commands.Create
{
    public class CreateInstructorCommunityCommandValidator : AbstractValidator<CreateInstructorCommunityCommand>
    {
        public CreateInstructorCommunityCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Platform)
               .NotNull().WithMessage("Platform is required.")
               .NotEmpty().WithMessage("Platform is required.");

            RuleFor(v => v.HandleName)
                .NotNull().WithMessage("Handle Name is required.")
                .NotEmpty().WithMessage("Handle Name is required.")
                .MinimumLength(2).WithMessage("Handle Name must be between 2 and 100 characters long.")
                .MaximumLength(100).WithMessage("Handle Name must be between 2 and 100 characters long.");
        }
    }
}
