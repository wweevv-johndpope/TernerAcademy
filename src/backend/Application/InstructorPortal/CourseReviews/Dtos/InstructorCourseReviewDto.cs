using System;

namespace Application.InstructorPortal.CourseReviews.Dtos
{
    public class InstructorCourseReviewDto
    {
        public string StudentName { get; set; }
        public string StudentProfilePictureUri { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
