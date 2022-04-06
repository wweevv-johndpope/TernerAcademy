using Application.Common.Models;
using Application.StudentPortal.CourseSubscriptions.Dtos;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CourseSubscriptionManager : ManagerBase, ICourseSubscriptionManager
    {
        private readonly ICourseSubscriptionWebService _courseWebService;
        public CourseSubscriptionManager(IManagerToolkit managerToolkit, ICourseSubscriptionWebService courseWebService) : base(managerToolkit)
        {
            _courseWebService = courseWebService;
        }

        public async Task<IResult<List<StudentCourseSubscriptionPurchaseItemDto>>> GetPurchaseHistoryAsync()
        {
            await PrepareForWebserviceCall();
            return await _courseWebService.GetPurchaseHistoryAsync(AccessToken);
        }

    }
}
