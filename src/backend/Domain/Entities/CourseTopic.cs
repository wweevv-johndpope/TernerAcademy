namespace Domain.Entities
{
    public class CourseTopic
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int TopicId { get; set; }
        public virtual CategoryTopic Topic { get; set; }
    }
}
