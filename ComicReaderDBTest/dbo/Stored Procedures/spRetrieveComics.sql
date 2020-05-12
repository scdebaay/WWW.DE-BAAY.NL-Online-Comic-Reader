CREATE PROCEDURE [dbo].[spRetrieveComics]
AS
SELECT * FROM [dbo].[Comics]
	WHERE [ComicDeleted] = 0
	ORDER BY [Path];