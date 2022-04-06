using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.InstructorPortal.CourseSubscriptions.Dtos;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.CourseSubscriptions.Queries.GetAll
{
    public class GetAllCourseSubscriptionsQuery : IRequest<Result<GetAllCourseSubscriptionsResponseDto>>
    {
        public int CourseId { get; set; }

        public class GetAllCourseSubscriptionsQueryHandler : IRequestHandler<GetAllCourseSubscriptionsQuery, Result<GetAllCourseSubscriptionsResponseDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageBlobService _blobService;
            public GetAllCourseSubscriptionsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IAzureStorageBlobService blobService)
            {
                _context = context;
                _dbContext = dbContext;
                _blobService = blobService;
            }

            public async Task<Result<GetAllCourseSubscriptionsResponseDto>> Handle(GetAllCourseSubscriptionsQuery request, CancellationToken cancellationToken)
            {
                var course = await _dbContext.Courses.AsQueryable().Include(x => x.Language).Include(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == request.CourseId && x.ListingStatus == CourseListingStatus.Approved && x.InstructorId == _context.UserId);

                if (course == null) return await Result<GetAllCourseSubscriptionsResponseDto>.FailAsync("Course not found.");

                var profilePictureContainerPath = _blobService.GetBlobContainerPath(BlobContainers.ProfilePhotos);

                var lessonCount = await _dbContext.CourseLessons.AsQueryable().CountAsync(x => x.CourseId == course.Id);

                var subscriptions = await _dbContext.InstructorCourseSubscriptionViewItems.AsQueryable().Where(x => x.CourseId == request.CourseId).OrderByDescending(x => x.DateSubscribed).ToListAsync();
                var totalEarnings = subscriptions.Where(x => x.AmountCashout.HasValue).Sum(x => x.AmountCashout.Value);

                var mappedSubscriptions = subscriptions.Select(x => new InstructorCourseSubscriptionDto()
                {
                    StudentName = x.StudentName,
                    StudentProfilePictureUri = !string.IsNullOrEmpty(x.StudentProfilePictureUri) ? $"{profilePictureContainerPath}/{x.StudentProfilePictureUri}" : "assets/default_photo.png",
                    Price = x.Price,
                    Progress = Convert.ToDouble(x.ViewLessons) / lessonCount,
                    DateSubscribed = x.DateSubscribed,
                    AmountCashout = x.AmountCashout,
                    CashoutPaymentTx = x.CashoutPaymentTx,
                    AmountBurn = x.AmountBurn,
                    BurnTx = x.BurnTx
                }).ToList();

                return await Result<GetAllCourseSubscriptionsResponseDto>.SuccessAsync(new GetAllCourseSubscriptionsResponseDto()
                {
                    Subscriptions = mappedSubscriptions,
                    TotalEarnings = totalEarnings,
                    SubscriptionCount = subscriptions.Count
                });
            }
        }
    }
}
