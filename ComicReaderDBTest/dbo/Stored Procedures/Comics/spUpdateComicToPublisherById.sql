-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Comic Publisher association records
-- =============================================
CREATE PROCEDURE spUpdateComicToPublisherById 
	-- Add the parameters for the stored procedure here
	@ComicId int,
    @PublisherId int    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToPublisher]
           SET
           [ComicId] = @ComicId
          ,[PublisherId] = @PublisherId
          
     WHERE [ComicId] = @ComicId
END