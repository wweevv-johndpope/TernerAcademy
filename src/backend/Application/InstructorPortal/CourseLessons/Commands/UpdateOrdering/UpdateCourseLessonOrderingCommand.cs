using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Commands.UpdateOrdering
{
    public class UpdateCourseLessonOrderingCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public bool IsUpward { get; set; }

        public class UpdateCourseLessonOrderingCommandHandler : IRequestHandler<UpdateCourseLessonOrderingCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public UpdateCourseLessonOrderingCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(UpdateCourseLessonOrderingCommand request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);

                if (course == null) return await Result.FailAsync("Course not found.");
                if (course.ListingStatus != CourseListingStatus.Draft) return await Result.FailAsync("Since course is not on draft mode, you cannot able to update course lesson ordering anymore.");

                var lesson = _dbContext.CourseLessons.AsQueryable().FirstOrDefault(x => x.CourseId == request.CourseId && x.Id == request.LessonId);
                var lessonOrder = _dbContext.CourseLessonOrders.Include(x => x.Lesson).AsQueryable().FirstOrDefault(x => x.LessonId == request.LessonId && x.Lesson.CourseId == request.CourseId );

                //Upwards
                if (request.IsUpward)
                {
                    var parentLessonOrder = _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).FirstOrDefault(x => x.Lesson.CourseId == request.CourseId && x.ChildLessonId == request.LessonId);

                    if (parentLessonOrder != null)
                    {
                        parentLessonOrder.ChildLessonId = lessonOrder.ChildLessonId;
                        _dbContext.CourseLessonOrders.Update(parentLessonOrder);
                        await _dbContext.SaveChangesAsync();

                        var parentParentLessonOrder = _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).FirstOrDefault(x => x.Lesson.CourseId == request.CourseId && x.ChildLessonId == parentLessonOrder.LessonId);

                        if (parentParentLessonOrder != null)
                        {
                            parentParentLessonOrder.ChildLessonId = lesson.Id;
                            _dbContext.CourseLessonOrders.Update(parentParentLessonOrder);
                            await _dbContext.SaveChangesAsync();
                        }

                        lessonOrder.ChildLessonId = parentLessonOrder.LessonId;
                        _dbContext.CourseLessonOrders.Update(lessonOrder);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    if (lessonOrder.ChildLessonId.HasValue)
                    {
                        var childLessonOrder = _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).FirstOrDefault(x => x.Lesson.CourseId == request.CourseId && x.LessonId == lessonOrder.ChildLessonId.Value);

                        if (childLessonOrder != null)
                        {
                            lessonOrder.ChildLessonId = childLessonOrder.ChildLessonId;
                            _dbContext.CourseLessons.Update(lesson);
                            await _dbContext.SaveChangesAsync();

                            var parentLessonOrder = _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).FirstOrDefault(x => x.Lesson.CourseId == request.CourseId && x.ChildLessonId == request.LessonId);

                            if (parentLessonOrder != null)
                            {
                                parentLessonOrder.ChildLessonId = childLessonOrder.LessonId;
                                _dbContext.CourseLessonOrders.Update(parentLessonOrder);
                                await _dbContext.SaveChangesAsync();
                            }

                            childLessonOrder.ChildLessonId = lesson.Id;
                            _dbContext.CourseLessonOrders.Update(childLessonOrder);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }


                return await Result.SuccessAsync();
            }
        }
    }
}
