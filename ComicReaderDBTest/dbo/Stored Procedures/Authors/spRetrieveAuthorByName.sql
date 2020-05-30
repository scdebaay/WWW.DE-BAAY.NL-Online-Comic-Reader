CREATE PROCEDURE [dbo].[spRetrieveAuthorByName]
	@LastName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Authors] WHERE [LastName] = @LastName AND [AuthorDeleted] = 0;
END