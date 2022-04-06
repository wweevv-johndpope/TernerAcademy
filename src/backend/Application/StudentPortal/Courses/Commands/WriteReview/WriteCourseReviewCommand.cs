using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.Commands.WriteReview
{
    public class WriteCourseReviewCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public class WriteReviewCommandHandler : IRequestHandler<WriteCourseReviewCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IDomainEventService _domainEventService;

            public WriteReviewCommandHandler(ICallContext context, IApplicationDbContext dbContext, IDomainEventService domainEventService)
            {
                _context = context;
                _dbContext = dbContext;
                _domainEventService = domainEventService;
            }

            public async Task<IResult> Handle(WriteCourseReviewCommand request, CancellationToken cancellationToken)
            {
                var subscription = await _dbContext.CourseSubscriptions.AsQueryable().FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.StudentId == _context.UserId);

                if (subscription == null) return await Result.FailAsync("You need to buy the course first before writing a review.");

                subscription.Rating = request.Rating;
                subscription.Comment = request.Comment;

                _dbContext.CourseSubscriptions.Update(subscription);
                await _dbContext.SaveChangesAsync();

                await _domainEventService.Publish(new NewCourseReviewEvent(request.CourseId));

                return await Result.SuccessAsync();
            }
        }
    }
}
