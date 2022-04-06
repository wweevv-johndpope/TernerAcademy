using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Commands.Delete
{
    public class DeleteCourseLessonCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }

        public class DeleteCourseLessonCommandHandler : IRequestHandler<DeleteCourseLessonCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public DeleteCourseLessonCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(DeleteCourseLessonCommand request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);

                if (course == null) return await Result.FailAsync("Course not found.");
                if (course.ListingStatus != CourseListingStatus.Draft) return await Result.FailAsync("Since course is not on draft mode, you cannot able to delete a course lesson anymore.");

                var lesson = await _dbContext.CourseLessons.AsQueryable().FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.Id == request.LessonId);
                if (lesson == null) return await Result.FailAsync("Course Lesson not found.");

                var lessonOrder = await _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).FirstOrDefaultAsync(x => x.Lesson.CourseId == request.CourseId && x.LessonId == request.LessonId);

                var parentLessonOrder = await _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).FirstOrDefaultAsync(x => x.Lesson.CourseId == request.CourseId && x.ChildLessonId == request.LessonId);

                if (parentLessonOrder != null)
                {
                    parentLessonOrder.ChildLessonId = lessonOrder.ChildLessonId;
                    _dbContext.CourseLessonOrders.Update(parentLessonOrder);
                    await _dbContext.SaveChangesAsync();
                }

                _dbContext.CourseLessons.Remove(lesson);
                _dbContext.CourseLessonOrders.Remove(lessonOrder);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
