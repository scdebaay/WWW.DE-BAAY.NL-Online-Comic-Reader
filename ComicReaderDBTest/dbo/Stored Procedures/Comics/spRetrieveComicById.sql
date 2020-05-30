CREATE PROCEDURE [dbo].[spRetrieveComicById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Comics] WHERE Id = @Id AND [ComicDeleted] = 0;
END