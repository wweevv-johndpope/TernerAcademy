namespace Domain.Entities
{
    public class StudentLesson
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int LessonId { get; set; }
        public virtual CourseLesson Lesson { get; set; }
    }
}
