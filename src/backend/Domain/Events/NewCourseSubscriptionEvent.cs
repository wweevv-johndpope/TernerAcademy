using Domain.Common;

namespace Domain.Events
{
    public class NewCourseSubscriptionEvent : DomainEvent
    {
        public int CourseId { get; private set; }

        public NewCourseSubscriptionEvent(int courseId)
        {
            CourseId = courseId;
        }
    }
}
