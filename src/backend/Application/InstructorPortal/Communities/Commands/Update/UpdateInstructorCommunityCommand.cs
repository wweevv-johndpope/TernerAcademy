using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Communities.Commands.Update
{
    public class UpdateInstructorCommunityCommand : IRequest<IResult>
    {
        public int CommunityId { get; set; }
        public string HandleName { get; set; }

        public class UpdateInstructorCommunityCommandHandler : IRequestHandler<UpdateInstructorCommunityCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public UpdateInstructorCommunityCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(UpdateInstructorCommunityCommand request, CancellationToken cancellationToken)
            {
                var community = await _dbContext.InstructorCommunities.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CommunityId && x.InstructorId == _context.UserId);

                if (community == null) return await Result.FailAsync("Community not found.");

                community.HandleName = request.HandleName;

                _dbContext.InstructorCommunities.Update(community);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
