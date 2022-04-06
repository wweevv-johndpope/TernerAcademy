using Domain.Common;

namespace Domain.Events
{
    public class NewCourseReviewEvent : DomainEvent
    {
        public int CourseId { get; private set; }

        public NewCourseReviewEvent(int courseId)
        {
            CourseId = courseId;
        }
    }
}
