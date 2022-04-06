using Application.Common.Mappings;
using Domain.Entities;

namespace Application.InstructorPortal.CourseLessons.Dtos
{
    public class InstructorCourseLessonDto : IMapFrom<CourseLesson>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsPreviewable { get; set; }

        public bool IsProcessing { get; set; }
        public string FinalVideoPathUri { get; set; }
        public int OrderIdx { get; set; }
        public int? ChildLessonId { get; set; }
        public long Duration { get; set; }

    }
}
