CREATE PROCEDURE [dbo].[spRemoveGenreFromComicToGenreByGenreId]
	@GenreId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToGenre]
           SET
           [AssocDeleted] = 1
     WHERE [GenreId] = @GenreId
END
