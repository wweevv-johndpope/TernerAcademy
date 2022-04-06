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

namespace Application.StudentPortal.Courses.Commands.Search
{
    public class SearchCourseCommand : IRequest<Result<List<StudentCourseItemDto>>>
    {
        public string SearchQuery { get; set; }

        public class SearchCourseQueryHandler : IRequestHandler<SearchCourseCommand, Result<List<StudentCourseItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            public SearchCourseQueryHandler(IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService, ICallContext context)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
                _context = context;
            }

            public async Task<Result<List<StudentCourseItemDto>>> Handle(SearchCourseCommand request, CancellationToken cancellationToken)
            {
                var availableCourses = await _dbContext.StudentCourseSubscriptionViewItems.AsQueryable().Where(x => !x.HasSubscription && x.StudentId == _context.UserId).ToListAsync();

                var courses = await _dbContext.StudentCourseViewItems.AsQueryable().ProjectTo<StudentCourseItemDto>(_mapper.ConfigurationProvider).ToListAsync();

                courses = courses.Where(x => availableCourses.Any(y => x.CourseId == y.CourseId)).ToList();

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

                return await Result<List<StudentCourseItemDto>>.SuccessAsync(courses);
            }
        }
    }
}
