using Application.Common.Constants;
using FluentValidation;

namespace Application.InstructorPortal.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.NewPassword)
                    .NotNull().WithMessage("New Password is required.")
                    .NotEmpty().WithMessage("New Password is required.")
                    .MinimumLength(AppConstants.MinimumPasswordLength).WithMessage($"New Password must be at least {AppConstants.MinimumPasswordLength} characters.");

            RuleFor(v => v.NewConfirmPassword)
                .NotNull().WithMessage("Confirm New Password is required.")
                .NotEmpty().WithMessage("Confirm New Password is required.")
                .Equal(v => v.NewPassword).WithMessage("New Password and Confirm New Password must be the same.");
        }
    }
}
