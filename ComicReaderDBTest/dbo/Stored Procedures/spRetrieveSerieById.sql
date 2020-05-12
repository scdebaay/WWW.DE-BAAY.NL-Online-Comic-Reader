CREATE PROCEDURE [dbo].[spRetrieveSerieById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Series] WHERE [Id] = @Id AND [SerieDeleted] = 0;
END