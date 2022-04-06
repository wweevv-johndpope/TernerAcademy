using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseTopicConfiguration : IEntityTypeConfiguration<CourseTopic>
    {
        public void Configure(EntityTypeBuilder<CourseTopic> builder)
        {
            builder.ToTable("Topics", "Course");
        }
    }
}
