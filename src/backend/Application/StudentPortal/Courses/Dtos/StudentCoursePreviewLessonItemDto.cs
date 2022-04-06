using Application.Common.Mappings;
using Domain.Entities;

namespace Application.StudentPortal.Courses.Dtos
{
    public class StudentCoursePreviewLessonItemDto : IMapFrom<CourseLesson>
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsPreviewable { get; set; }
        public string FinalVideoPathUri { get; set; }
        public long Duration { get; set; }
    }
}
