using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.Account.Dtos;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Account.Queries.MyProfile
{
    public class MyProfileQuery : IRequest<Result<StudentMyProfileDto>>
    {
        public class MyProfileQueryHandler : IRequestHandler<MyProfileQuery, Result<StudentMyProfileDto>>
        {
            private readonly ICallContext _context;
            private readonly IStudentIdentityService _identityService;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            public MyProfileQueryHandler(ICallContext context, IStudentIdentityService identityService, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _identityService = identityService;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<StudentMyProfileDto>> Handle(MyProfileQuery request, CancellationToken cancellationToken)
            {
                var user = await _identityService.GetAsync(_context.UserId);
                var mappedUser = _mapper.Map<StudentMyProfileDto>(user);

                mappedUser.ProfilePictureUri = "assets/default_photo.png";
                if (!string.IsNullOrWhiteSpace(user.ProfilePictureFilename))
                {
                    var containerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);
                    mappedUser.ProfilePictureUri = $"{containerPath}/{user.ProfilePictureFilename}";
                }

                return await Result<StudentMyProfileDto>.SuccessAsync(mappedUser);
            }
        }
    }
}
