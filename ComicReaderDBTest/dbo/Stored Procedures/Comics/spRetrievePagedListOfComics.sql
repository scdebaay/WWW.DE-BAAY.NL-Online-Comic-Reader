﻿CREATE PROCEDURE [dbo].[spRetrievePagedListOfComics]
	@pageLimit int,
	@page int = 0,
	@SearchText NVARCHAR(100)
AS
SELECT * FROM [dbo].[Comics] WHERE [ComicDeleted] = 0 AND @SearchText = '' OR 
	[ComicDeleted] = 0 AND [Name] LIKE '%'+@SearchText+'%' OR
	[ComicDeleted] = 0 AND [Path] LIKE '%'+@SearchText+'%'
	ORDER BY [Path]
	OFFSET @page*@pageLimit ROWS
	FETCH NEXT @pageLimit ROWS ONLY;