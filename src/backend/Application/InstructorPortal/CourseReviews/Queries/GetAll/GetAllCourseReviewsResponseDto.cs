using Application.InstructorPortal.CourseReviews.Dtos;
using System.Collections.Generic;

namespace Application.InstructorPortal.CourseReviews.Queries.GetAll
{
    public class GetAllCourseReviewsResponseDto
    {
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
        public List<InstructorCourseReviewDto> Reviews { get; set; } = new List<InstructorCourseReviewDto>();
    }
}
