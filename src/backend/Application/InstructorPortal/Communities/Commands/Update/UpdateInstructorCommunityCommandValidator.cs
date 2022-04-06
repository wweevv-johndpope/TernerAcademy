using FluentValidation;

namespace Application.InstructorPortal.Communities.Commands.Update
{
    public class UpdateInstructorCommunityCommandValidator : AbstractValidator<UpdateInstructorCommunityCommand>
    {
        public UpdateInstructorCommunityCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.HandleName)
                .NotNull().WithMessage("Handle Name is required.")
                .NotEmpty().WithMessage("Handle Name is required.")
                .MinimumLength(2).WithMessage("Handle Name must be between 2 and 100 characters long.")
                .MaximumLength(100).WithMessage("Handle Name must be between 2 and 100 characters long.");
        }
    }
}
