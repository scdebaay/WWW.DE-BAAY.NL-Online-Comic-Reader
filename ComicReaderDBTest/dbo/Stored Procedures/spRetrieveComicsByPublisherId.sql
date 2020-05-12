CREATE PROCEDURE [dbo].[spRetrieveComicsByPublisherId]
	@PublisherId int	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
    [dbo].[Comics].[Id]
FROM
    [dbo].[Comics]
INNER JOIN
    ([dbo].[Publisher] INNER JOIN [dbo].[ComicToPublisher] 
	ON [dbo].[Publisher].[Id] = [dbo].[ComicToPublisher].[PublisherId]) 
	ON [dbo].[Comics].[Id] = [dbo].[ComicToPublisher].[ComicId]
WHERE [dbo].[Publisher].[Id] = @PublisherId AND [ComicDeleted] = 0;
END