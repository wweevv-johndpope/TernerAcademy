using FluentValidation;

namespace Application.StudentPortal.Account.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Email)
             .NotNull().WithMessage("Email is required.")
             .NotEmpty().WithMessage("Email is required.");

            RuleFor(v => v.Password)
             .NotNull().WithMessage("Password is required.")
             .NotEmpty().WithMessage("Password is required.");
        }
    }
}
