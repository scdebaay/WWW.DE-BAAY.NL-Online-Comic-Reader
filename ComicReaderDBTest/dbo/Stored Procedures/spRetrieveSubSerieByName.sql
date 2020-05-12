CREATE PROCEDURE [dbo].[spRetrieveSubSerieByName]
	@Title NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[SubSeries] WHERE [Title] = @Title AND [SubSerieDeleted] = 0;
END