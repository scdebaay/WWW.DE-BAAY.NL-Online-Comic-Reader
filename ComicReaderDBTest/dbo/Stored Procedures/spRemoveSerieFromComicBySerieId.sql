CREATE PROCEDURE [dbo].[spRemoveSerieFromComicBySerieId]
	@SerieId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [SeriesId] = 1           
     WHERE [SeriesId] = @SerieId
END