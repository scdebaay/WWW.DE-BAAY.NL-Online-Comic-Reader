CREATE PROCEDURE [dbo].[spRetrieveComicsByAuthorId]
	@AuthorId int	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
    [dbo].[Comics].[Id]
FROM
    [dbo].[Comics]
INNER JOIN
    ([dbo].[Authors] INNER JOIN [dbo].[ComicToAuthors] 
	ON [dbo].[Authors].[Id] = [dbo].[ComicToAuthors].[AuthorId]) 
	ON [dbo].[Comics].[Id] = [dbo].[ComicToAuthors].[ComicId]
WHERE [dbo].[Authors].[Id] = @AuthorId AND [ComicDeleted] = 0;
END