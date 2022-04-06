SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StudentCourseSubscriptionView] AS
SELECT TT1.*, 
    CAST(CASE 
        WHEN TT2.Id IS NOT NULL THEN 1 
        ELSE 0 
    END AS BIT) AS HasSubscription
FROM 
    (
        SELECT T1.Id as CourseId, T2.Id as StudentId FROM Course.Courses T1
        CROSS JOIN Student.Students T2
        WHERE T1.ListingStatus = 2
    ) as TT1
    LEFT JOIN Course.Subscriptions TT2 ON TT1.CourseId = TT2.CourseId AND TT1.StudentId = TT2.StudentId
GO
