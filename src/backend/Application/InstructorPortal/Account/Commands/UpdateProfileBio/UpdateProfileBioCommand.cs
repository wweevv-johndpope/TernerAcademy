using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Account.Commands.UpdateProfileBio
{
    public class UpdateProfileBioCommand : IRequest<IResult>
    {
        public string Bio { get; set; }

        public class UpdateProfileBioCommandHandler : IRequestHandler<UpdateProfileBioCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IInstructorIdentityService _identityService;

            public UpdateProfileBioCommandHandler(ICallContext context, IInstructorIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(UpdateProfileBioCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.GetAsync(_context.UserId);

                user.Bio = request.Bio;

                return await _identityService.UpdateAsync(_context.UserId, user);
            }
        }
    }
}
