using Application.Common.Models;
using Application.StudentPortal.CourseSubscriptions.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CourseSubscriptionWebService : WebServiceBase, ICourseSubscriptionWebService
    {
        public CourseSubscriptionWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<StudentCourseSubscriptionPurchaseItemDto>>> GetPurchaseHistoryAsync(string accessToken) => GetAsync<List<StudentCourseSubscriptionPurchaseItemDto>>(CourseSubscriptionEndpoint.PurchaseHistory, accessToken);
    }
}
