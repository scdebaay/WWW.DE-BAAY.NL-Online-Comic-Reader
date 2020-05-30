CREATE PROCEDURE [dbo].[spRetrieveTypeByName]
	@Term NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Type] WHERE [Term] = @Term AND [TypeDeleted] = 0;
END