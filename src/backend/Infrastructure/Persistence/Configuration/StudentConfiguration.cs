using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students", "Student");

            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Email).HasMaxLength(100).IsRequired();
            builder.Property(t => t.EmailNormalize).HasMaxLength(100).IsRequired();

            builder.Property(t => t.ProfilePictureFilename).HasMaxLength(256);

            builder.HasIndex(t => t.Email).IsUnique();
            builder.HasIndex(t => t.EmailNormalize).IsUnique();
        }
    }
}
