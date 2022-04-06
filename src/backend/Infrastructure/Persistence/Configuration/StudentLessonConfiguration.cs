using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class StudentLessonConfiguration : IEntityTypeConfiguration<StudentLesson>
    {
        public void Configure(EntityTypeBuilder<StudentLesson> builder)
        {
            builder.ToTable("Lessons", "Student");
        }
    }
}
