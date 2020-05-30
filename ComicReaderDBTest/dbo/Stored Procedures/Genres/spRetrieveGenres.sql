CREATE PROCEDURE [dbo].[spRetrieveGenres]
AS
SELECT * FROM [dbo].[Genres] WHERE [GenreDeleted] = 0
	ORDER BY [Term];