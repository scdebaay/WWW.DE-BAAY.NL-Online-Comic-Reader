CREATE PROCEDURE [dbo].[spRetrieveSeries]
AS
SELECT * FROM [dbo].[Series] WHERE [SerieDeleted] = 0
	ORDER BY [Title];