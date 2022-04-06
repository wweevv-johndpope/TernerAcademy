SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CategoryTopicView] AS
SELECT T1.Id as CategoryId, T1.Name as Category, T2.Id as TopicId, T2.Name as Topic FROM [Category].Categories T1
JOIN [Category].Topics T2 ON T1.Id = T2.CategoryId
GO
