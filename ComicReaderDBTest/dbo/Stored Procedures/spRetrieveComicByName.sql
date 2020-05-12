CREATE PROCEDURE [dbo].[spRetrieveComicByName]
	@Name NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Comics] WHERE [Name] = @Name AND [ComicDeleted] = 0;
END