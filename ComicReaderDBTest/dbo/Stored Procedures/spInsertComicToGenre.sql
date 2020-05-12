-- =============================================
-- Author:		Solino de Baay
-- Create date: 30-04-2020
-- Description:	SP to insert Comic Author association records
-- =============================================
CREATE PROCEDURE spInsertComicToPublisher 
	-- Add the parameters for the stored procedure here
	@ComicId int,
    @PublisherId int    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ComicToPublisher]
			([ComicId]
			,[PublisherId]
			,[RecordUpdated])
           VALUES 
		   (@ComicId
           ,@PublisherId
		   ,CONVERT (date, CURRENT_TIMESTAMP))
END