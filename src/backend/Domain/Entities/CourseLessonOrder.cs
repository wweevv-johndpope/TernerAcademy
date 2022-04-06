namespace Domain.Entities
{
    public class CourseLessonOrder
    {
        public int Id { get; set; }

        public int LessonId { get; set; }
        public virtual CourseLesson Lesson { get; set; }

        public int? ChildLessonId { get; set; }
    }
}
