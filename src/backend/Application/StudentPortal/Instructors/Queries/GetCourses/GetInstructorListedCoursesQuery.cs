using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Courses.Dtos;
using Application.StudentPortal.Instructors.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Instructors.Queries.GetCourses
{
    public class GetInstructorListedCoursesQuery : IRequest<Result<List<StudentInstructorCourseDto>>>
    {
        public int InstructorId { get; set; }
        public class GetInstructorListedCoursesQueryHandler : IRequestHandler<GetInstructorListedCoursesQuery, Result<List<StudentInstructorCourseDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;

            public GetInstructorListedCoursesQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<List<StudentInstructorCourseDto>>> Handle(GetInstructorListedCoursesQuery request, CancellationToken cancellationToken)
            {
                var courses = await _dbContext.InstructorCourseViewItems
                    .AsQueryable()
                    .Where(x => x.InstructorId == request.InstructorId && x.ListingStatus == CourseListingStatus.Approved)
                    .ProjectTo<StudentInstructorCourseDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

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

                return await Result<List<StudentInstructorCourseDto>>.SuccessAsync(courses);
            }
        }
    }
}
