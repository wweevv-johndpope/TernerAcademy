using Application.InstructorPortal.CourseSubscriptions.Dtos;
using System.Collections.Generic;

namespace Application.InstructorPortal.CourseSubscriptions.Queries.GetAll
{
    public class GetAllCourseSubscriptionsResponseDto
    {
        public int SubscriptionCount { get; set; }
        public double TotalEarnings { get; set; }
        public List<InstructorCourseSubscriptionDto> Subscriptions { get; set; } = new List<InstructorCourseSubscriptionDto>();
    }
}
