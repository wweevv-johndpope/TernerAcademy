SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW  [dbo].[StudentEnrolledCourseView] AS
SELECT T1.StudentId,
       T3.Id as CourseId, 
       T3.Name as CourseName,
       T3.ShortDescription as CourseShortDescription,
       T3.ThumbnailImageUri as CourseThumbnailImageUri,
       T3.[Level] as CourseLevel,
       T4.Name as CourseLanguage,
       T5.Id as InstructorId,
       T5.Name as Instructor,
       T5.ProfilePictureFilename as InstructorProfilePictureUri,
       T1.CreatedAt as DateEnrolled

FROM Course.Subscriptions T1

JOIN Student.Students T2 ON T1.StudentId = T2.Id
JOIN Course.Courses T3 ON T1.CourseId = T3.Id
JOIN Course.Languages T4 ON T3.LanguageId = T4.Id
JOIN Instructor.Instructors T5 ON T3.InstructorId = T5.Id
GO
