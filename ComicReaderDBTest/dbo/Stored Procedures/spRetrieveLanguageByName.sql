CREATE PROCEDURE [dbo].[spRetrieveLanguageByName]
	@Term NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Language] WHERE [Term] = @Term AND [LanguageDeleted] = 0;
END