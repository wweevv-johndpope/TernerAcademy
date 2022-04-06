using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class InstructorPasswordConfiguration : IEntityTypeConfiguration<InstructorPassword>
    {
        public void Configure(EntityTypeBuilder<InstructorPassword> builder)
        {
            builder.ToTable("Passwords", "Instructor");

            builder.Property(p => p.Salt).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Digest).HasMaxLength(256).IsRequired();
        }
    }
}
