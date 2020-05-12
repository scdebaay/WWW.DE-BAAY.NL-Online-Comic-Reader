CREATE PROCEDURE [dbo].[spRetrieveSubSerieById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[SubSeries] WHERE [Id] = @Id AND [SubSerieDeleted] = 0;
END