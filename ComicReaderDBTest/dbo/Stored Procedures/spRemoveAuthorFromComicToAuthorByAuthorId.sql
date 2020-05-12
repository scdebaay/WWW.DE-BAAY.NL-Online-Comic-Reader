CREATE PROCEDURE [dbo].[spRemoveAuthorFromComicToAuthorByAuthorId]
	@AuthorId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToAuthors]
           SET
           [AssocDeleted] = 1
     WHERE [AuthorId] = @AuthorId
END