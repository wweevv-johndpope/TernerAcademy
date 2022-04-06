using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Instructors.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Instructors.Queries.Get
{
    public class GetInstructorDetailsQuery : IRequest<Result<StudentInstructorDto>>
    {
        public int InstructorId { get; set; }

        public class GetCourseInstructorDetailsQueryHandler : IRequestHandler<GetInstructorDetailsQuery, Result<StudentInstructorDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            public GetCourseInstructorDetailsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<StudentInstructorDto>> Handle(GetInstructorDetailsQuery request, CancellationToken cancellationToken)
            {
                var instructor = await _dbContext.Instructors.AsQueryable().FirstOrDefaultAsync(x => x.Id == request.InstructorId);

                if (instructor == null) return await Result<StudentInstructorDto>.FailAsync("Instructor not found.");

                var profilePictureContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);

                var mappedInstructor = _mapper.Map<StudentInstructorDto>(instructor);

                mappedInstructor.ProfilePictureUri = "assets/default_photo.png";

                if (!string.IsNullOrEmpty(instructor.ProfilePictureFilename))
                {
                    mappedInstructor.ProfilePictureUri = $"{profilePictureContainerPath}/{instructor.ProfilePictureFilename}";
                }

                var communities = await _dbContext.InstructorCommunities
                    .AsQueryable()
                    .Where(x => x.InstructorId == instructor.Id)
                    .ProjectTo<StudentInstructorCommunityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                mappedInstructor.Commmunities = communities;

                return await Result<StudentInstructorDto>.SuccessAsync(mappedInstructor);
            }
        }
    }
}
