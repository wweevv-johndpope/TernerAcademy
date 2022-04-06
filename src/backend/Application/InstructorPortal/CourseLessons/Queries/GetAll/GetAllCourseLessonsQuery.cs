using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Queries.GetAll
{
    public class GetAllCourseLessonsQuery : IRequest<Result<List<InstructorCourseLessonDto>>>
    {
        public int CourseId { get; set; }

        public class GetAllCourseLessonsQueryHandler : IRequestHandler<GetAllCourseLessonsQuery, Result<List<InstructorCourseLessonDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetAllCourseLessonsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<List<InstructorCourseLessonDto>>> Handle(GetAllCourseLessonsQuery request, CancellationToken cancellationToken)
            {
                var isCourseExists = await _dbContext.Courses.AsQueryable().AnyAsync(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);
                if (!isCourseExists) return await Result<List<InstructorCourseLessonDto>>.FailAsync("Course not found.");

                var lessons = await _dbContext.CourseLessons.AsQueryable().Where(x => x.CourseId == request.CourseId).ToListAsync();

                var lessonOrders = await _dbContext.CourseLessonOrders.AsQueryable().Include(x => x.Lesson).Where(x => x.Lesson.CourseId == request.CourseId).ToListAsync();

                if (lessonOrders.Any())
                {
                    var newLessons = new List<CourseLesson>();
                    var lastLessonOrder = lessonOrders.FirstOrDefault(x => !x.ChildLessonId.HasValue);

                    if (lastLessonOrder != null)
                    {
                        var lastLesson = lessons.First(x => x.Id == lastLessonOrder.LessonId);

                        newLessons.Add(lastLesson);

                        while (newLessons.Count != lessonOrders.Count)
                        {
                            var currentLessonOrder = lessonOrders.First(x => x.ChildLessonId.HasValue && x.ChildLessonId.Value == newLessons[0].Id);
                            var currentLesson = lessons.First(x => x.Id == currentLessonOrder.LessonId);
                            newLessons.Insert(0, currentLesson);
                        }
                    }

                    lessons = newLessons;
                }

                var mappedLessons = new List<InstructorCourseLessonDto>();

                foreach (var lesson in lessons)
                {
                    var mappedLesson = _mapper.Map<InstructorCourseLessonDto>(lesson);
                    mappedLesson.IsProcessing = string.IsNullOrEmpty(lesson.ThetaVideoPlayerUri);

                    if (!mappedLesson.IsProcessing)
                    {
                        mappedLesson.FinalVideoPathUri = lesson.ThetaVideoPlayerUri;
                        mappedLesson.Duration = lesson.ThetaVideoDuration;
                    }

                    mappedLessons.Add(mappedLesson);
                }


                return await Result<List<InstructorCourseLessonDto>>.SuccessAsync(mappedLessons);
            }
        }
    }
}
