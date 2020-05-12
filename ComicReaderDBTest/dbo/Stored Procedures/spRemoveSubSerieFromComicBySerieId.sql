CREATE PROCEDURE [dbo].[spRemoveSubSerieFromComicBySerieId]
	@SubSerieId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [SubSeriesId] = @SubSerieId
END