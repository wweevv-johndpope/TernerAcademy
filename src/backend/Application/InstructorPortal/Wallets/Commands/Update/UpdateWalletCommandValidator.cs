using FluentValidation;

namespace Application.InstructorPortal.Wallets.Commands.Update
{
    public class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
    {
        public UpdateWalletCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.WalletAddress)
               .NotNull().WithMessage("Wallet Address is required.")
               .NotEmpty().WithMessage("Wallet Address is required.")
               .Length(42).WithMessage("Wallet Address must starts with 0x and should be 20-byte address.");
        }
    }
}
