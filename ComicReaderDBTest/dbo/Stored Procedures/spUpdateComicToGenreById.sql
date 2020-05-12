-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Comic Genre association records
-- =============================================
CREATE PROCEDURE spUpdateComicToGenreById 
	-- Add the parameters for the stored procedure here
	@ComicId int,
    @GenreId int    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToGenre]
           SET
           [ComicId] = @ComicId
          ,[GenreId] = @GenreId
          
     WHERE [ComicId] = @ComicId
END