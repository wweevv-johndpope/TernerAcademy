using Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Views
{
    public class InstructorCourseSubscriptionViewConfiguration : IEntityTypeConfiguration<InstructorCourseSubscriptionViewItem>
    {
        public void Configure(EntityTypeBuilder<InstructorCourseSubscriptionViewItem> builder)
        {
            builder.ToView("InstructorCourseSubscriptionView")
                .HasNoKey();

            /*
            SELECT T1.Id AS CourseId,
                   T3.Name AS StudentName,
                   T3.ProfilePictureFilename AS StudentProfilePictureUri,
                   T2.CreatedAt AS DateSubscribed,
                   T2.Price AS Price,
                   COUNT(T5.StudentId) AS ViewLessons
            FROM Course.Courses T1
            JOIN Course.Subscriptions T2 ON T1.Id = T2.CourseId
            JOIN Student.Students T3 ON T3.Id = T2.StudentId
            JOIN Course.Lessons T4 ON T1.Id = T4.CourseId
            JOIN Student.Lessons T5 ON T5.StudentId = T2.StudentId AND T5.LessonId = T4.Id
            GROUP BY T1.Id, T3.Name, T3.ProfilePictureFilename, T2.CreatedAt, T2.Price
            */
        }
    }
}
