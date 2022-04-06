using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class StudentPasswordConfiguration : IEntityTypeConfiguration<StudentPassword>
    {
        public void Configure(EntityTypeBuilder<StudentPassword> builder)
        {
            builder.ToTable("Passwords", "Student");

            builder.Property(p => p.Salt).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Digest).HasMaxLength(256).IsRequired();
        }
    }
}
