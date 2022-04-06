using Domain.Common;

namespace Domain.Entities
{
    public class CourseLanguage : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNormalize { get; set; }
    }
}
