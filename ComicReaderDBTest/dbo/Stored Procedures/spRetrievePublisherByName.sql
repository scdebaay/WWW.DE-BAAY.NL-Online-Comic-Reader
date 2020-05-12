CREATE PROCEDURE [dbo].[spRetrievePublisherByName]
	@Name NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Publisher] WHERE [Name] = @Name AND [PublisherDeleted] = 0;
END