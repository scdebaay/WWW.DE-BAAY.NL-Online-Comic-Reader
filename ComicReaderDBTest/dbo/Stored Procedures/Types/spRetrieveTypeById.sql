CREATE PROCEDURE [dbo].[spRetrieveTypeById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Type] WHERE [Id] = @Id AND [TypeDeleted] = 0;
END