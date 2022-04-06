using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<IResult>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewConfirmPassword { get; set; }

        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IStudentIdentityService _identityService;

            public ChangePasswordCommandHandler(ICallContext context, IStudentIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var userId = _context.UserId;

                var result = await _identityService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);

                if (!result.Succeeded) return await Result.FailAsync("Invalid current password.");

                return await Result.SuccessAsync();
            }
        }
    }
}
