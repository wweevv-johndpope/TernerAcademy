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

namespace Application.StudentPortal.Courses.Queries.GetCoursePreview
{
    public class GetCoursePreviewQuery : IRequest<Result<StudentCoursePreviewDto>>
    {
        public int CourseId { get; set; }

        public class GetCoursePreviewQueryHandler : IRequestHandler<GetCoursePreviewQuery, Result<StudentCoursePreviewDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            public GetCoursePreviewQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<StudentCoursePreviewDto>> Handle(GetCoursePreviewQuery request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().Include(x => x.Language).Include(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == request.CourseId && x.ListingStatus == CourseListingStatus.Approved);

                if (course == null) return await Result<StudentCoursePreviewDto>.FailAsync("Course not found.");

                var courseViewItem = await _dbContext.StudentCourseViewItems.AsQueryable().FirstAsync(x => x.CourseId == request.CourseId);

                var profilePictureContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);
                var courseThumbnailContainerPath = _blobService.GetBlobContainerPath(BlobContainers.CourseThumbnails);

                var mappedCourse = _mapper.Map<StudentCoursePreviewDto>(course);
                mappedCourse.CourseLanguage = course.Language.Name;
                mappedCourse.LastUpdated = course.UpdatedAt.HasValue ? course.UpdatedAt.Value : course.CreatedAt;
                mappedCourse.ThumbnailImageUri = $"{courseThumbnailContainerPath}/{course.ThumbnailImageUri}";

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

                mappedCourse.IsEnrolled = await _dbContext.CourseSubscriptions.AsQueryable().AnyAsync(x => x.CourseId == request.CourseId && x.StudentId == _context.UserId);

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

                var mappedLessons = new List<StudentCoursePreviewLessonItemDto>();

                foreach (var lesson in lessons)
                {
                    var mappedLesson = _mapper.Map<StudentCoursePreviewLessonItemDto>(lesson);

                    if (mappedLesson.IsPreviewable)
                    {
                        mappedLesson.FinalVideoPathUri = lesson.ThetaVideoPlayerUri;
                    }

                    mappedLesson.Duration = lesson.ThetaVideoDuration;
                    mappedLessons.Add(mappedLesson);
                }

                mappedCourse.Lessons = mappedLessons;
                mappedCourse.Duration = lessons.Sum(x => x.ThetaVideoDuration);

                var reviews = await _dbContext.CourseSubscriptions.AsQueryable()
                    .Include(x => x.Student)
                    .Where(x => x.CourseId == request.CourseId && x.Rating.HasValue)
                    .OrderByDescending(x => x.UpdatedAt.Value)
                    .Take(10)
                    .Select(x => new StudentCoursePreviewReviewItemDto()
                    {
                        StudentName = x.Student.Name,
                        StudentProfilePictureUri = !string.IsNullOrEmpty(x.Student.ProfilePictureFilename) ? $"{profilePictureContainerPath}/{x.Student.ProfilePictureFilename}" : "assets/default_photo.png",
                        Rating = x.Rating.Value,
                        Comment = x.Comment,
                        LastUpdated = x.UpdatedAt.Value,
                    })
                    .ToListAsync();

                mappedCourse.Reviews = reviews;
                return await Result<StudentCoursePreviewDto>.SuccessAsync(mappedCourse);
            }
        }
    }
}
