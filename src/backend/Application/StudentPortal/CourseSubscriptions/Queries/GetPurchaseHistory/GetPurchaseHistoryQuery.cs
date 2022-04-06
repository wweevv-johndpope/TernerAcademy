using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.StudentPortal.CourseSubscriptions.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.CourseSubscriptions.Queries.GetPurchaseHistory
{
    public class GetPurchaseHistoryQuery : IRequest<Result<List<StudentCourseSubscriptionPurchaseItemDto>>>
    {
        public class GetPurchaseHistoryQueryHandler : IRequestHandler<GetPurchaseHistoryQuery, Result<List<StudentCourseSubscriptionPurchaseItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IAzureStorageBlobService _blobService;
            public GetPurchaseHistoryQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
                _blobService = blobService;
            }

            public async Task<Result<List<StudentCourseSubscriptionPurchaseItemDto>>> Handle(GetPurchaseHistoryQuery request, CancellationToken cancellationToken)
            {
                var purchases = await _dbContext.StudentCourseSubscriptionPurchaseViewItems
                    .AsQueryable()
                    .Where(x => x.StudentId == _context.UserId)
                    .OrderBy(x => x.DateBought)
                    .ProjectTo<StudentCourseSubscriptionPurchaseItemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                var profilePhotoContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);

                foreach (var purchase in purchases)
                {
                    if (!string.IsNullOrEmpty(purchase.InstructorProfilePictureUri))
                    {
                        purchase.InstructorProfilePictureUri = $"{profilePhotoContainerPath}/{purchase.InstructorProfilePictureUri}";
                    }
                    else
                    {
                        purchase.InstructorProfilePictureUri = "assets/default_photo.png";
                    }
                }

                return await Result<List<StudentCourseSubscriptionPurchaseItemDto>>.SuccessAsync(purchases);
            }
        }
    }
}
