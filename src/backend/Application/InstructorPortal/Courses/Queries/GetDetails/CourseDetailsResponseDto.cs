using Application.InstructorPortal.CourseLessons.Dtos;
using Application.InstructorPortal.Courses.Dtos;
using System.Collections.Generic;

namespace Application.InstructorPortal.Courses.Queries.GetDetails
{
    public class CourseDetailsResponseDto
    {
        public InstructorCourseDto Course { get; set; }
        public List<InstructorCourseLessonDto> Lessons { get; set; }
    }
}
