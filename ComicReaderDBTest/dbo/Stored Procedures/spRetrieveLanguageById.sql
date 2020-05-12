CREATE PROCEDURE [dbo].[spRetrieveLanguageById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Language] WHERE [Id] = @Id AND [LanguageDeleted] = 0;
END