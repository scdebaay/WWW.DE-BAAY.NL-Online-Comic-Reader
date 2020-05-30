CREATE PROCEDURE [dbo].[spRetrievePublishersByComicId]
	@Id int	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
    [dbo].[Publisher].[Id],
	[dbo].[Publisher].[Name]
FROM
    [dbo].[Publisher]
INNER JOIN
    ([dbo].[Comics] INNER JOIN [dbo].[ComicToPublisher] 
	ON [dbo].[Comics].[Id] = [dbo].[ComicToPublisher].[ComicId]) 
	ON [dbo].[Publisher].[Id] = [dbo].[ComicToPublisher].[PublisherId]
WHERE [dbo].[Comics].[Id] = @Id AND [PublisherDeleted] = 0;
END