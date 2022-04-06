SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[InstructorCourseSubscriptionView] AS

SELECT T1.Id AS CourseId,
       T3.Name AS StudentName,
       T3.ProfilePictureFilename AS StudentProfilePictureUri,
       T2.CreatedAt AS DateSubscribed,
       T2.Price AS Price,
       T2.CashoutPaymentTx AS CashoutPaymentTx,
       T2.AmountCashout AS AmountCashout,
       T2.SendToBurnTx AS BurnTx,
       T2.PriceBurn AS AmountBurn,
       COUNT(T5.StudentId) AS ViewLessons
FROM Course.Courses T1
JOIN Course.Subscriptions T2 ON T1.Id = T2.CourseId
JOIN Student.Students T3 ON T3.Id = T2.StudentId
JOIN Course.Lessons T4 ON T1.Id = T4.CourseId
LEFT JOIN Student.Lessons T5 ON T5.StudentId = T2.StudentId AND T5.LessonId = T4.Id
GROUP BY T1.Id, T3.Name, T3.ProfilePictureFilename, T2.CreatedAt, T2.Price, T2.CashoutPaymentTx, T2.AmountCashout, T2.SendToBurnTx, T2.PriceBurn
GO
