using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Courses.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Courses.Queries.GetAllListed
{
    public class GetAllListedCoursesQuery : IRequest<Result<List<InstructorListedCourseDto>>>
    {
        public class GetAllCoursesQueryHandler : IRequestHandler<GetAllListedCoursesQuery, Result<List<InstructorListedCourseDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetAllCoursesQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<List<InstructorListedCourseDto>>> Handle(GetAllListedCoursesQuery request, CancellationToken cancellationToken)
            {
                var courses = await _dbContext.InstructorCourseViewItems
                    .AsQueryable()
                    .Where(x => x.InstructorId == _context.UserId && x.ListingStatus == CourseListingStatus.Approved)
                    .ProjectTo<InstructorListedCourseDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return await Result<List<InstructorListedCourseDto>>.SuccessAsync(courses);
            }
        }
    }
}
