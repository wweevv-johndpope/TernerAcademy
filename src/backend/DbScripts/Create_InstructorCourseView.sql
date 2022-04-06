SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[InstructorCourseView] AS 
SELECT TT1.*, string_agg(T5.Name, ',') as Topics, string_agg(T5.Id, ',') as TopicIds FROM
(
    SELECT  T1.Id, 
            T1.InstructorId, 
            T1.Name, 
            T1.ShortDescription, 
            T1.Description, 
            T1.[Level], 
            T1.PriceInTFuel, 
            T1.ThumbnailImageUri,  
            T1.ListingStatus,
            T1.CreatedAt, 
            T1.EnrolledCount,
            T1.AverageRating,
            T1.RatingCount,
            T2.Id as [LanguageId], 
            T2.Name as [Language], 
            COUNT(T3.Id) as LessonCount ,  
            (CASE 
                    WHEN SUM(T3.ThetaVideoDuration) IS NULL THEN 0 
                    ELSE SUM(T3.ThetaVideoDuration) 
            END) as Duration 
    FROM Course.Courses T1
    JOIN Course.Languages T2 ON T2.Id = T1.LanguageId
    LEFT JOIN Course.Lessons T3 ON T3.CourseId = T1.Id
    GROUP BY T1.Id, T1.InstructorId, T1.Name, T1.ShortDescription, T1.Description, T1.[Level], T1.PriceInTFuel, T1.ThumbnailImageUri, T1.ListingStatus, T1.CreatedAt, T1.EnrolledCount, T1.AverageRating, T1.RatingCount, T2.Id, T2.Name
) AS TT1 
LEFT JOIN Course.Topics T4 ON T4.CourseId = TT1.Id
LEFT JOIN Category.Topics T5 ON T5.Id = T4.TopicId
GROUP BY TT1.Id, TT1.InstructorId, TT1.Name, TT1.ShortDescription, TT1.Description, TT1.[Level], TT1.PriceInTFuel, TT1.ThumbnailImageUri, TT1.ListingStatus, TT1.CreatedAt, TT1.EnrolledCount, TT1.AverageRating, TT1.RatingCount, TT1.LanguageId, TT1.Language, TT1.LessonCount, TT1.Duration
GO
