using Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Views
{
    public class StudentEnrolledCourseViewConfiguration : IEntityTypeConfiguration<StudentEnrolledCourseViewItem>
    {
        public void Configure(EntityTypeBuilder<StudentEnrolledCourseViewItem> builder)
        {
            builder.ToView("StudentEnrolledCourseView")
                .HasNoKey();

            /*
CREATE VIEW StudentEnrolledCourseView AS
SELECT T1.StudentId,
       T3.Id as CourseId, 
       T3.Name as CourseName,
       T3.ShortDescription as CourseShortDescription,
       T3.ThumbnailImageUri as CourseThumbnailImageUri,
       T3.[Level] as CourseLevel,
       T4.Name as CourseLanguage,
       T5.Id as InstructorId,
       T5.Name as Instructor,
       T5.ProfilePictureFilename as InstructorProfilePictureUri

FROM Course.Subscriptions T1

JOIN Student.Students T2 ON T1.StudentId = T2.Id
JOIN Course.Courses T3 ON T1.CourseId = T3.Id
JOIN Course.Languages T4 ON T3.LanguageId = T4.Id
JOIN Instructor.Instructors T5 ON T3.InstructorId = T5.Id

             */
        }
    }
}
