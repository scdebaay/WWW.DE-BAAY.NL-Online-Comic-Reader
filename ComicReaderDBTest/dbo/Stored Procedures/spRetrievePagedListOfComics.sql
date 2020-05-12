CREATE PROCEDURE [dbo].[spRetrievePagedListOfComics]
	@pageLimit int,
	@page int = 0
AS
SELECT * FROM [dbo].[Comics] WHERE [ComicDeleted] = 0
	ORDER BY [Path]
	OFFSET @page*@pageLimit ROWS
	FETCH NEXT @pageLimit ROWS ONLY;