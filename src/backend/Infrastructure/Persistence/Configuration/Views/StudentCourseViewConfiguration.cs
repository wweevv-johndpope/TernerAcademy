using Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Views
{
    public class StudentCourseViewConfiguration : IEntityTypeConfiguration<StudentCourseViewItem>
    {
        public void Configure(EntityTypeBuilder<StudentCourseViewItem> builder)
        {
            builder.ToView("StudentCourseView")
                .HasKey(x => x.CourseId);

            /*
SELECT TT1.*, string_agg(T5.Name, ',') as CourseTopics, string_agg(T5.Id, ',') as CourseTopicIds FROM
(
    SELECT T1.Id as CourseId, 
           T1.Name as CourseName,
           T1.ShortDescription as CourseShortDescription,
           T1.ThumbnailImageUri as CourseThumbnailImageUri,
           T1.[Level] as CourseLevel,
           T1.PriceInTFuel as CoursePriceInTFuel,
           T1.ListingStatus as CourseListingStatus,
           T1.AverageRating as CourseAverageRating,
           T1.EnrolledCount as CourseEnrolledCount,
           T1.CreatedAt as CourseCreatedAt,
           T2.Id as [CourseLanguageId], 
           T2.Name as [CourseLanguage], 
           T3.Id as InstructorId,
           T3.Name as InstructorName,
           T3.ProfilePictureFilename as InstructorProfilePictureUri,
           COUNT(T4.Id) as LessonCount,
           CASE 
                WHEN SUM(T4.ThetaVideoDuration) IS NULL 
                THEN 0 ELSE SUM(T4.ThetaVideoDuration) 
            END  as Duration 
    FROM Course.Courses T1
    JOIN Course.Languages T2 ON T1.LanguageId = T2.Id
    JOIN Instructor.Instructors T3 ON T1.InstructorId = T3.Id
    LEFT JOIN Course.Lessons T4 ON T1.Id = T4.CourseId
    GROUP BY T1.Id, T1.Name, T1.ShortDescription, T1.ThumbnailImageUri, T1.[Level], T1.PriceInTFuel, T1.ListingStatus,  T1.AverageRating, T1.EnrolledCount, T1.CreatedAt,  T2.Id, T2.Name, T3.Id, T3.Name, T3.ProfilePictureFilename
) AS TT1 
LEFT JOIN Course.Topics T4 ON T4.CourseId = TT1.CourseId
LEFT JOIN Category.Topics T5 ON T5.Id = T4.TopicId
WHERE TT1.CourseListingStatus = 2
GROUP BY TT1.CourseId, TT1.CourseName, TT1.CourseShortDescription, TT1.CourseThumbnailImageUri, TT1.CourseLevel, TT1.CoursePriceInTFuel, TT1.CourseListingStatus,  TT1.CourseAverageRating, TT1.CourseEnrolledCount, TT1.CourseCreatedAt,  TT1.CourseLanguageId, TT1.CourseLanguage, TT1.InstructorId, TT1.InstructorName, TT1.InstructorProfilePictureUri, TT1.LessonCount, TT1.Duration
             */
        }
    }
}
