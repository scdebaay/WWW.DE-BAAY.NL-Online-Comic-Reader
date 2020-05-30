CREATE PROCEDURE [dbo].[spRetrieveGenreByName]
	@Term NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Genres] WHERE [Term] = @Term AND [GenreDeleted] = 0;
END