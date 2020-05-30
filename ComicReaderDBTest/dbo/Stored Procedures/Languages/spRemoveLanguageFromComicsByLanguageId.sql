CREATE PROCEDURE [dbo].[spRemoveLanguageFromComicsByLanguageId]
	@LanguageId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [LanguageId] = 1           
     WHERE [LanguageId] = @LanguageId
END