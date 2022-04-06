using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Courses.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Queries.GetAll
{
    public class GetAllCoursesQuery : IRequest<Result<List<InstructorCourseItemDto>>>
    {
        public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, Result<List<InstructorCourseItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;

            public GetAllCoursesQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<List<InstructorCourseItemDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
            {
                var courses = await _dbContext.InstructorCourseViewItems.AsQueryable().Where(x => x.InstructorId == _context.UserId).ProjectTo<InstructorCourseItemDto>(_mapper.ConfigurationProvider).ToListAsync();

                var containerPath = _blobService.GetBlobContainerPath(BlobContainers.CourseThumbnails);
                foreach (var course in courses)
                {
                    if (!string.IsNullOrEmpty(course.ThumbnailImageUri))
                    {
                        course.ThumbnailImageUri = $"{containerPath}/{course.ThumbnailImageUri}";
                    }
                    else
                    {
                        course.ThumbnailImageUri = "assets/default_course_thumbnail.jpg";
                    }
                }

                return await Result<List<InstructorCourseItemDto>>.SuccessAsync(courses);
            }
        }
    }
}
