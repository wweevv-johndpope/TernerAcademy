using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Commands.Publish
{
    public class PublishCourseCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }

        public class PublishCourseCommandHandler : IRequestHandler<PublishCourseCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IInstructorIdentityService _identityService;
            public PublishCourseCommandHandler(ICallContext context, IApplicationDbContext dbContext, IInstructorIdentityService identityService)
            {
                _context = context;
                _dbContext = dbContext;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(PublishCourseCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.GetAsync(_context.UserId);

                if (string.IsNullOrEmpty(user.WalletAddress)) return await Result.FailAsync("To be able to publish a course, you need to add a wallet first. It can be found on the upper right corner of your screen.");

                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.InstructorId == _context.UserId && x.Id == request.CourseId);

                if (course == null) return await Result.FailAsync("Course not found.");

                if (course.ListingStatus != CourseListingStatus.Draft) return await Result.FailAsync("Course has been already published.");

                if (string.IsNullOrEmpty(course.ThumbnailImageUri)) return await Result.FailAsync("Course Thumbnail is required.");

                var courseLessonCount = await _dbContext.CourseLessons.CountAsync(x => x.CourseId == request.CourseId);
                if (courseLessonCount < 3) return await Result.FailAsync("To be able to publish a course, it needs to have at least 3 lessons it in.");

                var hasLessonStillProcessing = await _dbContext.CourseLessons.AnyAsync(x => x.CourseId == request.CourseId && string.IsNullOrEmpty(x.ThetaVideoPlayerUri));
                if (hasLessonStillProcessing) return await Result.FailAsync("Some of the lessons are still processing.");

                course.ListingStatus = CourseListingStatus.Approved;

                _dbContext.Courses.Update(course);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
