using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseLanguageConfiguration : IEntityTypeConfiguration<CourseLanguage>
    {
        public void Configure(EntityTypeBuilder<CourseLanguage> builder)
        {
            builder.ToTable("Languages", "Course");

            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
            builder.Property(t => t.NameNormalize).HasMaxLength(50).IsRequired();

            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasIndex(t => t.NameNormalize).IsUnique();
        }
    }
}
