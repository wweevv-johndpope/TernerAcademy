using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.Commands.WatchLesson
{
    public class WatchLessonCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }

        public class WatchLessonCommandHandler : IRequestHandler<WatchLessonCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public WatchLessonCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(WatchLessonCommand request, CancellationToken cancellationToken)
            {
                var isExists = await _dbContext.CourseLessons.AsQueryable().AnyAsync(x => x.Id == request.LessonId && x.CourseId == request.CourseId);

                if (!isExists) return await Result.FailAsync();

                var alreadyWatched = await _dbContext.StudentLessons.AsQueryable().AnyAsync(x => x.LessonId == request.LessonId && x.StudentId == _context.UserId);

                if (!alreadyWatched)
                {
                    var lesson = new StudentLesson()
                    {
                        LessonId = request.LessonId,
                        StudentId = _context.UserId
                    };

                    _dbContext.StudentLessons.Add(lesson);
                    await _dbContext.SaveChangesAsync();
                }

                return await Result.SuccessAsync();
            }
        }
    }
}
