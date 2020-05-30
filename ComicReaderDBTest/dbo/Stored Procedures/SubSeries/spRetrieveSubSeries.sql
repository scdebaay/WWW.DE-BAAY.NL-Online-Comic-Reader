CREATE PROCEDURE [dbo].[spRetrieveSubSeries]
AS
SELECT * FROM [dbo].[SubSeries] WHERE [SubSerieDeleted] = 0
	ORDER BY [Title];