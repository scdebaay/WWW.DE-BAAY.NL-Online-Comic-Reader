CREATE PROCEDURE [dbo].[spRetrieveLanguages]
AS
SELECT * FROM [dbo].[Language]
WHERE [LanguageDeleted] != 1
	ORDER BY [Term];