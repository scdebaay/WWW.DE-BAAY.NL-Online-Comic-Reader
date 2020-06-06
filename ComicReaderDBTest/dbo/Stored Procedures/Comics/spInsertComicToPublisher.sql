-- =============================================
-- Author:		Solino de Baay
-- Create date: 06-06-2020
-- Description:	SP to insert Comic Author association records
-- =============================================
CREATE PROCEDURE spInsertComicToPublisher
	-- Add the parameters for the stored procedure here
	@Ids uComicIdPropIdTVP READONLY   
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[ComicToPublisher]
			([ComicId]
			,[PublisherId]
			,[RecordUpdated])
           SELECT *,CONVERT (date, CURRENT_TIMESTAMP)
		FROM @Ids ids
	WHERE NOT EXISTS (SELECT * FROM [ComicToPublisher] existing WHERE existing.ComicId = ids.Id AND existing.PublisherId = ids.PropId);
END