using System;

namespace Application.StudentPortal.Courses.Dtos
{
    public class StudentCoursePreviewReviewItemDto
    {
        public string StudentName { get; set; }
        public string StudentProfilePictureUri { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
