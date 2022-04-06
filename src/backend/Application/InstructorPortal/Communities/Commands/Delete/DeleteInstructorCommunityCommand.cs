using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Communities.Commands.Delete
{
    public class DeleteInstructorCommunityCommand : IRequest<IResult>
    {
        public int CommunityId { get; set; }

        public class DeleteInstructorCommunityCommandHandler : IRequestHandler<DeleteInstructorCommunityCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public DeleteInstructorCommunityCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(DeleteInstructorCommunityCommand request, CancellationToken cancellationToken)
            {
                var community = await _dbContext.InstructorCommunities.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CommunityId && x.InstructorId == _context.UserId);

                if (community == null) return await Result.FailAsync("Community not found.");

                _dbContext.InstructorCommunities.Remove(community);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
