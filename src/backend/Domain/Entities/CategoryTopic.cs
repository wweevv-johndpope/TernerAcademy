namespace Domain.Entities
{
    public class CategoryTopic
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public string Name { get; set; }
        public string NameNormalize { get; set; }
    }
}
