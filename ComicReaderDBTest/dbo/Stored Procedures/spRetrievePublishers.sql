CREATE PROCEDURE [dbo].[spRetrievePublishers]
AS
SELECT * FROM [dbo].[Publisher] WHERE [PublisherDeleted] = 0
	ORDER BY [Name];
