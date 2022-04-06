using Application.Common.Mappings;
using Domain.Entities;

namespace Application.StudentPortal.Courses.Dtos
{
    public class StudentCourseDetailsLessonItemDto : IMapFrom<CourseLesson>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string FinalVideoPathUri { get; set; }
        public bool IsWatched { get; set; }
        public long Duration { get; set; }
    }
}
