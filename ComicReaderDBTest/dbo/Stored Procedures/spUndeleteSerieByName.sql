CREATE PROCEDURE [dbo].[spUndeleteSerieByName]
	@Title NVARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Series]
           SET
           [RecordDeleted] = NULL,
		   [SerieDeleted] = 0
     WHERE [Title] = @Title
	 RETURN IDENT_CURRENT('Series')
END