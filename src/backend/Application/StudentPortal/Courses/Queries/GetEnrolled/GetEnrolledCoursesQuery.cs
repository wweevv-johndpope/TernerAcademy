using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Courses.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Courses.Queries.GetEnrolled
{
    public class GetEnrolledCoursesQuery : IRequest<Result<List<StudentEnrolledCourseItemDto>>>
    {
        public class GetEnrolledCoursesQueryHandler : IRequestHandler<GetEnrolledCoursesQuery, Result<List<StudentEnrolledCourseItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            public GetEnrolledCoursesQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<List<StudentEnrolledCourseItemDto>>> Handle(GetEnrolledCoursesQuery request, CancellationToken cancellationToken)
            {
                var courses = await _dbContext.StudentEnrolledCourseViewItems.AsQueryable().Where(x => x.StudentId == _context.UserId).ProjectTo<StudentEnrolledCourseItemDto>(_mapper.ConfigurationProvider).ToListAsync();

                var courseThumbnailcontainerPath = _blobService.GetBlobContainerPath(BlobContainers.CourseThumbnails);
                var profilePhotoContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);
                foreach (var course in courses)
                {
                    course.CourseThumbnailImageUri = $"{courseThumbnailcontainerPath}/{course.CourseThumbnailImageUri}";

                    if (!string.IsNullOrEmpty(course.InstructorProfilePictureUri))
                    {
                        course.InstructorProfilePictureUri = $"{profilePhotoContainerPath}/{course.InstructorProfilePictureUri}";
                    }
                    else
                    {
                        course.InstructorProfilePictureUri = "assets/default_photo.png";
                    }
                }

                return await Result<List<StudentEnrolledCourseItemDto>>.SuccessAsync(courses);
            }
        }
    }
}
