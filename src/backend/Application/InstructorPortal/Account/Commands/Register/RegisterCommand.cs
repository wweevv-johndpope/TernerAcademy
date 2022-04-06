using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Account.Commands.Register
{
    public class RegisterCommand : IRequest<IResult>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IInstructorIdentityService _identityService;

            public RegisterCommandHandler(ICallContext context, IInstructorIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var user = new Instructor()
                {
                    Email = request.Email,
                    Name = request.Name,
                    CompanyName = request.CompanyName,
                };

                var createAccountResult = await _identityService.CreateAsync(user);

                if (createAccountResult.Succeeded)
                {
                    await _identityService.SetPasswordAsync(createAccountResult.Data, request.Password);
                    return await Result.SuccessAsync();
                }

                return await Result.FailAsync(createAccountResult.Messages);
            }
        }
    }
}
