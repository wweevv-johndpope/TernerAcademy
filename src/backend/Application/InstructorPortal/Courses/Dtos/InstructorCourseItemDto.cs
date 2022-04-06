using Application.Common.Mappings;
using Domain.Enums;
using Domain.Views;
using System;

namespace Application.InstructorPortal.Courses.Dtos
{
    public class InstructorCourseItemDto : IMapFrom<InstructorCourseViewItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Level { get; set; }
        public double PriceInTFuel { get; set; }
        public string ThumbnailImageUri { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public int EnrolledCount { get; set; }
        public string Language { get; set; }
        public string Topics { get; set; }
        public int LessonCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public CourseListingStatus ListingStatus { get; set; }
        public long Duration { get; set; }

    }
}
