-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Comic Author association records
-- =============================================
CREATE PROCEDURE spUpdateComicToAuthorById 
	-- Add the parameters for the stored procedure here
	@ComicId int,
    @AuhtorId int    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToAuthors]
           SET
           [ComicId] = @ComicId
          ,[AuthorId] = @AuhtorId
          
     WHERE [ComicId] = @ComicId
END