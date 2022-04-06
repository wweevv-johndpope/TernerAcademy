using Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Views
{
    public class StudentCourseSubscriptionViewConfiguration : IEntityTypeConfiguration<StudentCourseSubscriptionViewItem>
    {
        public void Configure(EntityTypeBuilder<StudentCourseSubscriptionViewItem> builder)
        {
            builder.ToView("StudentCourseSubscriptionView")
                .HasNoKey();

            /*
SELECT TT1.*, 
    CASE 
        WHEN TT2.Id IS NOT NULL THEN 1 
        ELSE 0 
    END AS HasSubscription
FROM 
    (
        SELECT T1.Id as CourseId, T2.Id as StudentId FROM Course.Courses T1
        CROSS JOIN Student.Students T2
        WHERE T1.ListingStatus = 2
    ) as TT1
    LEFT JOIN Course.Subscriptions TT2 ON TT1.CourseId = TT2.CourseId AND TT1.StudentId = TT2.StudentId
             */
        }
    }
}
