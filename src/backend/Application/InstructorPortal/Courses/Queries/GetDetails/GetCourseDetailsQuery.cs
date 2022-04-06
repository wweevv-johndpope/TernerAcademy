using Application.Common.Constants;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseLessons.Dtos;
using Application.InstructorPortal.Courses.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Queries.GetDetails
{
    public class GetCourseDetailsQuery : IRequest<Result<CourseDetailsResponseDto>>
    {
        public int CourseId { get; set; }

        public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, Result<CourseDetailsResponseDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;

            public GetCourseDetailsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<CourseDetailsResponseDto>> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.InstructorCourseViewItems.AsQueryable().FirstOrDefaultAsync(x => x.InstructorId == _context.UserId && x.Id == request.CourseId);

                if (course == null) return await Result<CourseDetailsResponseDto>.FailAsync("Course not found.");

                var mappedCourse = _mapper.Map<InstructorCourseDto>(course);

                var containerPath = _blobService.GetBlobContainerPath(BlobContainers.CourseThumbnails);
                if (!string.IsNullOrEmpty(course.ThumbnailImageUri))
                {
                    mappedCourse.ThumbnailImageUri = $"{containerPath}/{course.ThumbnailImageUri}";
                }
                else
                {
                    mappedCourse.ThumbnailImageUri = "assets/default_course_thumbnail.jpg";
                }

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

                return await Result<CourseDetailsResponseDto>.SuccessAsync(new CourseDetailsResponseDto()
                {
                    Course = mappedCourse,
                    Lessons = mappedLessons
                });
            }
        }
    }
}
