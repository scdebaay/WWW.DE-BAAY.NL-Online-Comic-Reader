-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to Delete association records
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteComicToGenreById] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[ComicToGenre]
           SET
		   [AssocDeleted] = 1
     WHERE [ComicId] = @Id
END