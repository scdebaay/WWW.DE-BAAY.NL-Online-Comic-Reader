CREATE PROCEDURE [dbo].[spRetrieveAuthorById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Authors] WHERE Id = @Id AND [AuthorDeleted] = 0;
END