using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Commands.Update
{
    public class UpdateCourseLessonCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string LessonNotes { get; set; }
        public bool LessonIsPreviewable { get; set; }

        public class UpdateCourseLessonCommandHandler : IRequestHandler<UpdateCourseLessonCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public UpdateCourseLessonCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(UpdateCourseLessonCommand request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);

                if (course == null) return await Result.FailAsync("Course not found.");

                var lesson = await _dbContext.CourseLessons.AsQueryable().FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.Id == request.LessonId);
                if (lesson == null) return await Result.FailAsync("Course Lesson not found.");

                lesson.Name = request.LessonName;
                lesson.Notes = request.LessonNotes;
                lesson.IsPreviewable = request.LessonIsPreviewable;
                _dbContext.CourseLessons.Update(lesson);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
