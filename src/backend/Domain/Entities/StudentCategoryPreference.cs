namespace Domain.Entities
{
    public class StudentCategoryPreference
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
