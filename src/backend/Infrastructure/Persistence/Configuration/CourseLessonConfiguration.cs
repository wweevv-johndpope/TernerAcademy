using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseLessonConfiguration : IEntityTypeConfiguration<CourseLesson>
    {
        public void Configure(EntityTypeBuilder<CourseLesson> builder)
        {
            builder.ToTable("Lessons", "Course");

            builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
            builder.Property(t => t.VideoPathUri).HasMaxLength(100).IsRequired();
            builder.Property(t => t.ThetaVideoPlayerUri).HasMaxLength(1024);
            builder.Property(t => t.ThetaVideoPlaybackUri).HasMaxLength(1024);
            builder.Property(t => t.ThetaVideoId).HasMaxLength(256);

            builder.HasIndex(t => t.Name);
        }
    }
}
