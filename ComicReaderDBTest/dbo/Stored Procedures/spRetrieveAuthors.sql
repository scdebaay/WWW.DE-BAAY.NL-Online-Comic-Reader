CREATE PROCEDURE [dbo].[spRetrieveAuthors]
AS
SELECT * FROM [dbo].[Authors] WHERE [AuthorDeleted] = 0
	ORDER BY [LastName];