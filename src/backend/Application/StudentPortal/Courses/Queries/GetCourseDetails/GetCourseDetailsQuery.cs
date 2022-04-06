using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Courses.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.Queries.GetCourseDetails
{
    public class GetCourseDetailsQuery : IRequest<Result<StudentCourseDetailsDto>>
    {
        public int CourseId { get; set; }

        public class GetCourseDetailsQueryHandler : IRequestHandler<GetCourseDetailsQuery, Result<StudentCourseDetailsDto>>
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

            public async Task<Result<StudentCourseDetailsDto>> Handle(GetCourseDetailsQuery request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().Include(x => x.Language).Include(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == request.CourseId && x.ListingStatus == CourseListingStatus.Approved);

                if (course == null) return await Result<StudentCourseDetailsDto>.FailAsync("Course not found.");

                var subscription = await _dbContext.CourseSubscriptions.AsQueryable().FirstOrDefaultAsync(x => x.CourseId == request.CourseId && x.StudentId == _context.UserId);

                if (subscription == null) return await Result<StudentCourseDetailsDto>.FailAsync("You need to buy the course first before viewing its details.");

                var courseViewItem = await _dbContext.StudentCourseViewItems.AsQueryable().FirstAsync(x => x.CourseId == request.CourseId);

                var profilePictureContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);

                var mappedCourse = _mapper.Map<StudentCourseDetailsDto>(course);
                mappedCourse.CourseLanguage = course.Language.Name;
                mappedCourse.LastUpdated = course.UpdatedAt.HasValue ? course.UpdatedAt.Value : course.CreatedAt;

                mappedCourse.InstructorId = course.Instructor.Id;
                mappedCourse.InstructorName = course.Instructor.Name;
                mappedCourse.InstructorCompanyName = course.Instructor.CompanyName;
                mappedCourse.InstructorProfilePictureUri = "assets/default_photo.png";

                if (!string.IsNullOrEmpty(course.Instructor.ProfilePictureFilename))
                {
                    mappedCourse.InstructorProfilePictureUri = $"{profilePictureContainerPath}/{course.Instructor.ProfilePictureFilename}";
                }

                mappedCourse.Topics = courseViewItem.CourseTopics;
                mappedCourse.TopicIds = courseViewItem.CourseTopicIds;

                mappedCourse.MyCourseSubscriptionRating = subscription.Rating;
                mappedCourse.MyCourseSubscriptionComment = subscription.Comment ?? string.Empty;

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

                var studentLessons = await _dbContext.StudentLessons.AsQueryable().Include(x => x.Lesson).Where(x => x.StudentId == _context.UserId && x.Lesson.CourseId == request.CourseId).ToListAsync();

                var mappedLessons = new List<StudentCourseDetailsLessonItemDto>();

                foreach (var lesson in lessons)
                {
                    var mappedLesson = _mapper.Map<StudentCourseDetailsLessonItemDto>(lesson);
                    mappedLesson.FinalVideoPathUri = lesson.ThetaVideoPlayerUri;
                    mappedLesson.Duration = lesson.ThetaVideoDuration;
                    mappedLesson.IsWatched = studentLessons.Any(x => x.LessonId == lesson.Id);
                    mappedLessons.Add(mappedLesson);
                }

                mappedCourse.Lessons = mappedLessons;
                mappedCourse.Duration = lessons.Sum(x => x.ThetaVideoDuration);

                var reviews = await _dbContext.CourseSubscriptions.AsQueryable()
                   .Include(x => x.Student)
                   .Where(x => x.CourseId == request.CourseId && x.Rating.HasValue)
                   .OrderByDescending(x => x.UpdatedAt.Value)
                   .Take(10)
                   .Select(x => new StudentCourseDetailsReviewItemDto()
                   {
                       StudentName = x.Student.Name,
                       StudentProfilePictureUri = !string.IsNullOrEmpty(x.Student.ProfilePictureFilename) ? $"{profilePictureContainerPath}/{x.Student.ProfilePictureFilename}" : "assets/default_photo.png",
                       Rating = x.Rating.Value,
                       Comment = x.Comment ?? string.Empty,
                       LastUpdated = x.UpdatedAt.Value,
                   })
                   .ToListAsync();

                mappedCourse.Reviews = reviews;

                return await Result<StudentCourseDetailsDto>.SuccessAsync(mappedCourse);
            }
        }
    }
}
