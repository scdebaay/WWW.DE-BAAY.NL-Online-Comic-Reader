CREATE PROCEDURE [dbo].[spRetrieveTypes]
AS
SELECT * FROM [dbo].[Type] WHERE [TypeDeleted] = 0
	ORDER BY [Term];