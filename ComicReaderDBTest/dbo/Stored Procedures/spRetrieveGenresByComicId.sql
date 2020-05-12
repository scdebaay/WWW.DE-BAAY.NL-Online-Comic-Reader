CREATE PROCEDURE [dbo].[spRetrieveGenresByComicId]
	@Id int	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
    [dbo].[Genres].[Id],
	[dbo].[Genres].[Term]
FROM
    [dbo].[Genres]
INNER JOIN
    ([dbo].[Comics] INNER JOIN [dbo].[ComicToGenre] 
	ON [dbo].[Comics].[Id] = [dbo].[ComicToGenre].[ComicId]) 
	ON [dbo].[Genres].[Id] = [dbo].[ComicToGenre].[GenreId]
WHERE [dbo].[Comics].[Id] = @Id  AND [GenreDeleted] = 0;
END