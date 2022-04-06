using Application.Common.Constants;
using FluentValidation;

namespace Application.InstructorPortal.Account.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .MinimumLength(3).WithMessage("Email must be between 3 and 100 characters long.")
                .MaximumLength(100).WithMessage("Email must be between 3 and 100 characters long.")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be between 3 and 100 characters long.")
                .MaximumLength(100).WithMessage("Name must be between 3 and 100 characters long.");

            //RuleFor(v => v.CompanyName)
            //    .NotNull().WithMessage("Company Name is required.")
            //    .NotEmpty().WithMessage("Company Name is required.")
            //    .MinimumLength(3).WithMessage("Company Name must be between 3 and 256 characters long.")
            //    .MaximumLength(256).WithMessage("Company Name must be between 3 and 256 characters long.");

            RuleFor(v => v.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(AppConstants.MinimumPasswordLength).WithMessage($"Password must be at least {AppConstants.MinimumPasswordLength} characters long.");

            RuleFor(v => v.ConfirmPassword)
                .NotNull().WithMessage("Confirm Password is required.")
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(v => v.Password).WithMessage("Password and Confirm Password must be the same.");
        }
    }
}
