using Application.Common.Mappings;
using Domain.Views;
using System;

namespace Application.StudentPortal.Courses.Dtos
{
    public class StudentEnrolledCourseItemDto : IMapFrom<StudentEnrolledCourseViewItem>
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        //public string CourseShortDescription { get; set; }
        public string CourseThumbnailImageUri { get; set; }
        public string CourseLevel { get; set; }
        public string CourseLanguage { get; set; }
        public int InstructorId { get; set; }
        public string Instructor { get; set; }
        public string InstructorProfilePictureUri { get; set; }
        public DateTime DateEnrolled { get; set; }
    }
}
