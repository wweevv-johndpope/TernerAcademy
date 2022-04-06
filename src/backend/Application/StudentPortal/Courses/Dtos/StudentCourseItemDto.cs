using Application.Common.Mappings;
using Domain.Views;

namespace Application.StudentPortal.Courses.Dtos
{
    public class StudentCourseItemDto : IMapFrom<StudentCourseViewItem>
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        //public string CourseShortDescription { get; set; }
        public string CourseThumbnailImageUri { get; set; }
        public string CourseLevel { get; set; }
        public double CoursePriceInTFuel { get; set; }
        //public CourseListingStatus CourseListingStatus { get; set; }
        public double CourseAverageRating { get; set; }
        public int CourseRatingCount { get; set; }
        public int CourseEnrolledCount { get; set; }
        //public DateTime CourseCreatedAt { get; set; }
        public int CourseLanguageId { get; set; }
        public string CourseLanguage { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorProfilePictureUri { get; set; }
        public int LessonCount { get; set; }
        public long Duration { get; set; }
        public string CourseTopics { get; set; }
        public string CourseTopicIds { get; set; }
    }
}
