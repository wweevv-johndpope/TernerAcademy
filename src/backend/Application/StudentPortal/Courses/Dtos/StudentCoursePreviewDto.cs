using Application.Common.Mappings;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.StudentPortal.Courses.Dtos
{
    public class StudentCoursePreviewDto : IMapFrom<Course>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public double PriceInTFuel { get; set; }
        public string ThumbnailImageUri { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public int EnrolledCount { get; set; }
        public string CourseLanguage { get; set; }
        public long Duration { get; set; }
        public DateTime LastUpdated { get; set; }

        public string Topics { get; set; }
        public string TopicIds { get; set; }

        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorCompanyName { get; set; }
        public string InstructorProfilePictureUri { get; set; }

        public bool IsEnrolled { get; set; }

        public List<StudentCoursePreviewLessonItemDto> Lessons { get; set; }
        public List<StudentCoursePreviewReviewItemDto> Reviews { get; set; }
    }
}
