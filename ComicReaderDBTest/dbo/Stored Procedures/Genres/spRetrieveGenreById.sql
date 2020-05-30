CREATE PROCEDURE [dbo].[spRetrieveGenreById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Genres] WHERE [Id] = @Id AND [GenreDeleted] = 0;
END