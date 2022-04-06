using Application.Common.Mappings;
using Domain.Views;

namespace Application.StudentPortal.Instructors.Dtos
{
    public class StudentInstructorCourseDto : IMapFrom<InstructorCourseViewItem>
    {
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
        public long Duration { get; set; }
    }
}
