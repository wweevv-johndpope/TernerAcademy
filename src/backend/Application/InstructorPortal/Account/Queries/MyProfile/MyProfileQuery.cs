using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.Account.Dtos;
using Application.InstructorPortal.Communities.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Account.Queries.MyProfile
{
    public class MyProfileQuery : IRequest<Result<InstructorMyProfileDto>>
    {
        public class MyProfileQueryHandler : IRequestHandler<MyProfileQuery, Result<InstructorMyProfileDto>>
        {
            private readonly ICallContext _context;
            private readonly IInstructorIdentityService _identityService;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IApplicationDbContext _dbContext;

            public MyProfileQueryHandler(ICallContext context, IInstructorIdentityService identityService, IMapper mapper, IAzureStorageBlobService blobService, IApplicationDbContext dbContext)
            {
                _context = context;
                _identityService = identityService;
                _mapper = mapper;
                _blobService = blobService;
                _dbContext = dbContext;
            }

            public async Task<Result<InstructorMyProfileDto>> Handle(MyProfileQuery request, CancellationToken cancellationToken)
            {
                var user = await _identityService.GetAsync(_context.UserId);
                var mappedUser = _mapper.Map<InstructorMyProfileDto>(user);

                mappedUser.ProfilePictureUri = "assets/default_photo.png";
                if (!string.IsNullOrWhiteSpace(user.ProfilePictureFilename))
                {
                    var containerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);
                    mappedUser.ProfilePictureUri = $"{containerPath}/{user.ProfilePictureFilename}";
                }

                var communities = await _dbContext.InstructorCommunities.AsQueryable().Where(x => x.InstructorId == _context.UserId).ProjectTo<InstructorCommunityDto>(_mapper.ConfigurationProvider).ToListAsync();
                mappedUser.Communities = communities;

                return await Result<InstructorMyProfileDto>.SuccessAsync(mappedUser);
            }
        }
    }
}
