using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstructorPortal.Dashboard.Queries.Get
{
    public class GetDashboardQuery : IRequest<Result<GetDashboardResponseDto>>
    {
        public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, Result<GetDashboardResponseDto>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public GetDashboardQueryHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<Result<GetDashboardResponseDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
            {
                var registerStudentCount = await _dbContext.Students.AsQueryable().CountAsync();
                var totalListedCourseCount = await _dbContext.Courses.AsQueryable().CountAsync(x => x.ListingStatus == CourseListingStatus.Approved);
                var totalAmountBurned = await _dbContext.CourseSubscriptions.AsQueryable().Where(x => x.PriceBurn.HasValue).SumAsync(x => x.PriceBurn.Value);
                var totalAmountPayouts = await _dbContext.CourseSubscriptions.AsQueryable().Where(x => x.AmountCashout.HasValue).SumAsync(x => x.AmountCashout.Value);

                var courseCount = await _dbContext.Courses.AsQueryable().CountAsync(x => x.ListingStatus == CourseListingStatus.Approved && x.InstructorId == _context.UserId);
                var courseLessonCount = await _dbContext.CourseLessons.Include(x => x.Course).AsQueryable().CountAsync(x => x.Course.ListingStatus == CourseListingStatus.Approved && x.Course.InstructorId == _context.UserId);
                var coursePurchaseCount = await _dbContext.CourseSubscriptions.Include(x => x.Course).AsQueryable().CountAsync(x => x.Course.InstructorId == _context.UserId);
                var courseWithRatings = await _dbContext.Courses.AsQueryable().Where(x => x.ListingStatus == CourseListingStatus.Approved && x.InstructorId == _context.UserId && x.RatingCount != 0).Select(x => new { AverageRating = x.AverageRating }).ToListAsync();
                var averageRating = courseWithRatings.Count > 0 ? courseWithRatings.Sum(x => x.AverageRating) / courseWithRatings.Count : 0;
                var totalEarnings = await _dbContext.CourseSubscriptions.AsQueryable().Include(x => x.Course).Where(x => x.Course.InstructorId == _context.UserId && x.AmountCashout.HasValue).SumAsync(x => x.AmountCashout.Value);
                var uniqueStudents = _dbContext.CourseSubscriptions.AsQueryable().Include(x => x.Course).Where(x => x.Course.InstructorId == _context.UserId).AsEnumerable().DistinctBy(x => x.StudentId).Count();

                return await Result<GetDashboardResponseDto>.SuccessAsync(new GetDashboardResponseDto()
                {
                    RegisteredStudentCount = registerStudentCount,
                    TotalListedCourseCount = totalListedCourseCount,
                    TotalAmountPayouts = totalAmountPayouts,
                    TotalAmountBurned = totalAmountBurned,

                    InstructorCourseCount = courseCount,
                    InstructorCourseLessonCount = courseLessonCount,
                    InstructorCoursePurchaseCount = coursePurchaseCount,
                    InstructorAverageRating = averageRating,
                    InstructorTotalEarnings = totalEarnings,
                    InstructorUniqueStudentCount = uniqueStudents
                });
            }
        }
    }
}
