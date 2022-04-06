using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseReviews.Dtos;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseReviews.Queries.GetAll
{
    public class GetAllCourseReviewsQuery : IRequest<Result<GetAllCourseReviewsResponseDto>>
    {
        public int CourseId { get; set; }

        public class GetAllCourseReviewsQueryHandler : IRequestHandler<GetAllCourseReviewsQuery, Result<GetAllCourseReviewsResponseDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageBlobService _blobService;
            public GetAllCourseReviewsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _blobService = blobService;
            }

            public async Task<Result<GetAllCourseReviewsResponseDto>> Handle(GetAllCourseReviewsQuery request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().Include(x => x.Language).Include(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == request.CourseId && x.ListingStatus == CourseListingStatus.Approved && x.InstructorId == _context.UserId);

                if (course == null) return await Result<GetAllCourseReviewsResponseDto>.FailAsync("Course not found.");

                var profilePictureContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);

                var reviews = await _dbContext.CourseSubscriptions.AsQueryable()
                  .Include(x => x.Student)
                  .Where(x => x.CourseId == request.CourseId && x.Rating.HasValue)
                  .OrderByDescending(x => x.UpdatedAt.Value)
                  .Select(x => new InstructorCourseReviewDto()
                  {
                      StudentName = x.Student.Name,
                      StudentProfilePictureUri = !string.IsNullOrEmpty(x.Student.ProfilePictureFilename) ? $"{profilePictureContainerPath}/{x.Student.ProfilePictureFilename}" : "assets/default_photo.png",
                      Rating = x.Rating.Value,
                      Comment = x.Comment,
                      LastUpdated = x.UpdatedAt.Value,
                  })
                  .ToListAsync();

                return await Result<GetAllCourseReviewsResponseDto>.SuccessAsync(new GetAllCourseReviewsResponseDto()
                {
                    AverageRating = course.AverageRating,
                    RatingCount = course.RatingCount,
                    Reviews = reviews
                });
            }
        }
    }
}
