-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE spUpdateComicToLanguageById 
	-- Add the parameters for the stored procedure here
	@Id int,    
    @LanguageId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [LanguageId] = @LanguageId           
     WHERE [Id] = @Id
END