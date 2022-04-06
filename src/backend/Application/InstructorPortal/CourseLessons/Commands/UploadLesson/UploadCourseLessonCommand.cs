using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseLessons.Commands.UploadLesson
{
    public class UploadCourseLessonCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public string LessonName { get; set; }
        public string LessonNotes { get; set; }
        public bool LessonIsPreviewable { get; set; }
        public Stream FileStream { get; set; }
        public string FileExtension { get; set; }

        public class UploadCourseLessonCommandHandler : IRequestHandler<UploadCourseLessonCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageQueueService _queueService;

            public UploadCourseLessonCommandHandler(ICallContext context, IAzureStorageBlobService blobService, IApplicationDbContext dbContext, IAzureStorageQueueService queueService)
            {
                _context = context;
                _blobService = blobService;
                _dbContext = dbContext;
                _queueService = queueService;
            }

            public async Task<IResult> Handle(UploadCourseLessonCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.AllowableVideoFormat.Split(",").Any(x => x.Trim() == request.FileExtension.ToLower()))
                {
                    return await Result.FailAsync("Format not supported.");
                }

                var course = await _dbContext.Courses.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);

                if (course == null) return await Result.FailAsync("Course not found.");
                if (course.ListingStatus != CourseListingStatus.Draft) return await Result.FailAsync("Since course is not on draft mode, you cannot able to add a course lesson anymore.");

                string filename = $"{Guid.NewGuid()}{request.FileExtension}".ToLower();
                await _blobService.UploadAsync(request.FileStream, BlobContainers.CourseLessons, filename);

                var lesson = new CourseLesson()
                {
                    CourseId = request.CourseId,
                    Name = request.LessonName,
                    Notes = request.LessonNotes,
                    VideoPathUri = filename,
                    IsPreviewable = request.LessonIsPreviewable
                };

                _dbContext.CourseLessons.Add(lesson);
                _dbContext.SaveChanges();

                _dbContext.CourseLessonOrders.Add(new CourseLessonOrder() { LessonId = lesson.Id });
                _dbContext.SaveChanges();

                var lastChild = await _dbContext.CourseLessonOrders
                    .AsQueryable()
                    .Include(x => x.Lesson)
                    .ThenInclude(x => x.Course)
                    .FirstOrDefaultAsync(x => x.Lesson.CourseId == request.CourseId && x.LessonId != lesson.Id && x.ChildLessonId == null);

                if (lastChild != null)
                {
                    lastChild.ChildLessonId = lesson.Id;
                    _dbContext.CourseLessonOrders.Update(lastChild);
                    _dbContext.SaveChanges();
                }

                _queueService.InsertMessage(QueueNames.Transcoder, $"{lesson.Id}");

                return await Result.SuccessAsync();
            }
        }
    }
}
