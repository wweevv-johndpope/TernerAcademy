using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Wallets.Commands.Update
{
    public class UpdateWalletCommand : IRequest<IResult>
    {
        public string WalletAddress { get; set; }

        public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IInstructorIdentityService _identityService;

            public UpdateWalletCommandHandler(ICallContext context, IInstructorIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
            {
                return await _identityService.UpdateWalletAddressAsync(_context.UserId, request.WalletAddress);
            }
        }
    }
}
