CREATE PROCEDURE [dbo].[spRetrievePublisherById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Publisher] WHERE [Id] = @Id AND [PublisherDeleted] = 0;
END