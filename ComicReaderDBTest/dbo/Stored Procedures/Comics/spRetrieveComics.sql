CREATE PROCEDURE [dbo].[spRetrieveComics]
	@SearchText NVARCHAR(100)
AS
BEGIN
SELECT * FROM [dbo].[Comics]
	WHERE [ComicDeleted] = 0 AND @SearchText = '' OR 
	[ComicDeleted] = 0 AND [Name] LIKE '%'+@SearchText+'%' OR
	[ComicDeleted] = 0 AND [Path] LIKE '%'+@SearchText+'%'
	ORDER BY [Path];
END