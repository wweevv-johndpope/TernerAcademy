using Application.Common.Models;
using Application.StudentPortal.CourseSubscriptions.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICourseSubscriptionWebService : IWebService
    {
        Task<IResult<List<StudentCourseSubscriptionPurchaseItemDto>>> GetPurchaseHistoryAsync(string accessToken);
    }
}