-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to insert Comic Author association records
-- =============================================
CREATE PROCEDURE spInsertComicToAuthor 
	-- Add the parameters for the stored procedure here
	@ComicId int,
    @AuthorId int    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ComicToAuthors]
			([ComicId]
			,[AuthorId]
			,[RecordUpdated])
           VALUES 
		   (@ComicId
           ,@AuthorId
		   ,CONVERT (date, CURRENT_TIMESTAMP))
END