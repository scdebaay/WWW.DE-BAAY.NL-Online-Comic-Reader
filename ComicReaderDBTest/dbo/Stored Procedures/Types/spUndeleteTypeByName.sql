CREATE PROCEDURE [dbo].[spUndeleteTypeByName]
	@Term NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Type]
           SET
           [RecordDeleted] = NULL,
		   [TypeDeleted] = 0
     WHERE [Term] = @Term
	 RETURN IDENT_CURRENT('Type')
END