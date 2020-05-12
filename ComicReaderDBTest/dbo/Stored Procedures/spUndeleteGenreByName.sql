CREATE PROCEDURE [dbo].[spUndeleteGenreByName]
	@Term NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Genres]
           SET
           [RecordDeleted] = NULL,
		   [GenreDeleted] = 0
     WHERE [Term] = @Term
	 RETURN IDENT_CURRENT('Genres')
END