CREATE PROCEDURE [dbo].[spRetrieveSerieByName]
	@Title NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Series] WHERE [Title] = @Title AND [SerieDeleted] = 0;
END