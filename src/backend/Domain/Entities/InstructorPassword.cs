using Domain.Common;

namespace Domain.Entities
{
    public class InstructorPassword : AuditableEntity
    {
        public int Id { get; set; }

        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }

        public string Salt { get; set; }

        public string Digest { get; set; }

        public bool IsCurrent { get; set; }

    }
}
