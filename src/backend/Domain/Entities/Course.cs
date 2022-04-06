using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Course : AuditableEntity
    {
        public int Id { get; set; }

        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }

        public int LanguageId { get; set; }
        public virtual CourseLanguage Language { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public double PriceInTFuel { get; set; }
        public string ThumbnailImageUri { get; set; }

        public CourseListingStatus ListingStatus { get; set; }

        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public int EnrolledCount { get; set; }
    }
}
