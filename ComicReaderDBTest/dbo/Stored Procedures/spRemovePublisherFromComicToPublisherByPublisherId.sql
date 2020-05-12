CREATE PROCEDURE [dbo].[spRemovePublisherFromComicToPublisherByPublisherId]
	@PublisherId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToPublisher]
           SET
           [AssocDeleted] = 1
     WHERE [PublisherId] = @PublisherId
END
