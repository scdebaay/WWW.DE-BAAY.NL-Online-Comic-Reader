CREATE PROCEDURE [dbo].[spUndeleteLanguageByName]
	@Term NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Language]
           SET
           [RecordDeleted] = NULL,
		   [LanguageDeleted] = 0
     WHERE [Term] = @Term
	 RETURN IDENT_CURRENT('Language')
END