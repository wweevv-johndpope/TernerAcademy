using Application.Common.Models;
using Application.StudentPortal.CourseSubscriptions.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICourseSubscriptionManager : IManager
    {
        Task<IResult<List<StudentCourseSubscriptionPurchaseItemDto>>> GetPurchaseHistoryAsync();
    }
}