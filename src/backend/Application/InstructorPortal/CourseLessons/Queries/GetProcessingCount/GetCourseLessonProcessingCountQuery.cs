using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Queries.GetProcessingCount
{
    public class GetCourseLessonProcessingCountQuery : IRequest<Result<int>>
    {
        public int CourseId { get; set; }

        public class GetCourseLessonProcessingCountQueryHandler : IRequestHandler<GetCourseLessonProcessingCountQuery, Result<int>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            public GetCourseLessonProcessingCountQueryHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<Result<int>> Handle(GetCourseLessonProcessingCountQuery request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.InstructorId == _context.UserId && x.Id == request.CourseId);

                if (course == null) return await Result<int>.FailAsync("Course not found.");

                var lessonCount = await _dbContext.CourseLessons.CountAsync(x => x.CourseId == request.CourseId && string.IsNullOrEmpty(x.ThetaVideoPlayerUri));

                return await Result<int>.SuccessAsync(lessonCount);
            }
        }
    }
}
