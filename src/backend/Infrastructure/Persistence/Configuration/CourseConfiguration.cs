using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses", "Course");

            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.ShortDescription).HasMaxLength(100).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(2048).IsRequired();
            builder.Property(t => t.Level).HasMaxLength(20).IsRequired();
            builder.Property(t => t.ThumbnailImageUri).HasMaxLength(256);

            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasIndex(t => t.Level);
        }
    }
}
