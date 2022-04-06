using FluentValidation;

namespace Application.InstructorPortal.Account.Commands.UpdateProfileBio
{
    public class UpdateProfileBioCommandValidator : AbstractValidator<UpdateProfileBioCommand>
    {
        public UpdateProfileBioCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Bio)
                .MaximumLength(2048).WithMessage("Bio must be less than 2048 characters.");
        }
    }
}
