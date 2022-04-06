using Domain.Common;

namespace Domain.Entities
{
    public class StudentPassword : AuditableEntity
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public string Salt { get; set; }

        public string Digest { get; set; }

        public bool IsCurrent { get; set; }
    }
}
