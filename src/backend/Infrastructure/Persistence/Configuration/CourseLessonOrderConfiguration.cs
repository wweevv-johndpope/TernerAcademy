using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CourseLessonOrderConfiguration : IEntityTypeConfiguration<CourseLessonOrder>
    {
        public void Configure(EntityTypeBuilder<CourseLessonOrder> builder)
        {
            builder.ToTable("LessonOrders", "Course");
        }
    }
}
