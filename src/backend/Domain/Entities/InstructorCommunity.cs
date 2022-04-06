namespace Domain.Entities
{
    public class InstructorCommunity
    {
        public int Id { get; set; }

        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }

        public string Platform { get; set; }
        public string HandleName { get; set; }
    }
}
