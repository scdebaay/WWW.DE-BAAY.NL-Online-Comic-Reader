-- =============================================
-- Author:		Solino de Baay
-- Create date: 06-06-2020
-- Description:	SP to insert Comic Author association records
-- =============================================
CREATE PROCEDURE spInsertComicToGenre
	-- Add the parameters for the stored procedure here
	@Ids uComicIdPropIdTVP READONLY  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO [dbo].[ComicToGenre]
			([ComicId]
			,[GenreId]
			,[RecordUpdated])
        SELECT *,CONVERT (date, CURRENT_TIMESTAMP)
		FROM @Ids ids
	WHERE NOT EXISTS (SELECT * FROM [ComicToGenre] existing WHERE existing.ComicId = ids.Id AND existing.GenreId = ids.PropId);
END