CREATE PROCEDURE [dbo].[spRetrieveComicsByGenreId]
	@GenreId int	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
    [dbo].[Comics].[Id]
FROM
    [dbo].[Comics]
INNER JOIN
    ([dbo].[Genres] INNER JOIN [dbo].[ComicToGenre] 
	ON [dbo].[Genres].[Id] = [dbo].[ComicToGenre].[GenreId]) 
	ON [dbo].[Comics].[Id] = [dbo].[ComicToGenre].[ComicId]
WHERE [dbo].[Genres].[Id] = @GenreId AND [GenreDeleted] = 0;
END