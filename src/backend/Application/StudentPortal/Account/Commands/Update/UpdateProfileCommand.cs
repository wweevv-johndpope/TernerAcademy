using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Account.Commands.Update
{
    public class UpdateProfileCommand : IRequest<IResult>
    {
        public string Name { get; set; }

        public class UpdateAccountCommandHandler : IRequestHandler<UpdateProfileCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IStudentIdentityService _identityService;

            public UpdateAccountCommandHandler(ICallContext context, IStudentIdentityService identityService)
            {
                _context = context;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.GetAsync(_context.UserId);

                user.Name = request.Name;

                return await _identityService.UpdateAsync(_context.UserId, user);
            }
        }
    }
}
