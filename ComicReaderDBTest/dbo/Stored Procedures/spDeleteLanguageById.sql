-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to Delete Language records
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteLanguageById] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Language]
           SET
		   [LanguageDeleted] = 1
     WHERE [Id] = @Id
END