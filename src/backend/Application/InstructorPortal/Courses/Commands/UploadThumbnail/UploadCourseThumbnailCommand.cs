using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Commands.UploadThumbnail
{
    public class UploadCourseThumbnailCommand : IRequest<IResult>
    {
        public int CourseId { get; set; }
        public Stream FileStream { get; set; }
        public string FileExtension { get; set; }

        public class UploadCourseThumbnailCommandHandler : IRequestHandler<UploadCourseThumbnailCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IApplicationDbContext _dbContext;
            private readonly IInstructorIdentityService _identityService;

            public UploadCourseThumbnailCommandHandler(ICallContext context, IAzureStorageBlobService blobService, IApplicationDbContext dbContext, IInstructorIdentityService identityService)
            {
                _context = context;
                _blobService = blobService;
                _dbContext = dbContext;
                _identityService = identityService;
            }

            public async Task<IResult> Handle(UploadCourseThumbnailCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.AllowableImageFormat.Split(",").Any(x => x.Trim() == request.FileExtension.ToLower()))
                {
                    return await Result.FailAsync("Format not supported.");
                }

                var course = _dbContext.Courses.AsQueryable().FirstOrDefault(x => x.Id == request.CourseId && x.InstructorId == _context.UserId);

                if (course == null) return await Result.FailAsync("Course not found.");
                if (course.ListingStatus != CourseListingStatus.Draft) return await Result.FailAsync("Course Thumbnail cannot be change anymore.");

                string filename = $"{Guid.NewGuid()}{request.FileExtension}".ToLower();
                await _blobService.UploadAsync(request.FileStream, BlobContainers.CourseThumbnails, filename);

                course.ThumbnailImageUri = filename;

                _dbContext.Courses.Update(course);
                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
