using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Communities.Commands.Create
{
    public class CreateInstructorCommunityCommand : IRequest<IResult>
    {
        public string Platform { get; set; }
        public string HandleName { get; set; }

        public class CreateInstructorCommunityHandler : IRequestHandler<CreateInstructorCommunityCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public CreateInstructorCommunityHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(CreateInstructorCommunityCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.CommunityPlatforms.Any(x => x == request.Platform)) return await Result.FailAsync("Invalid Community Platform");

                _dbContext.InstructorCommunities.Add(new InstructorCommunity()
                {
                    InstructorId = _context.UserId,
                    Platform = request.Platform,
                    HandleName = request.HandleName,
                });

                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
