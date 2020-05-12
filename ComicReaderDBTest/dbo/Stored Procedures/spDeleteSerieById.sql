-- =============================================
-- Author:		Solino de Baay
-- Create date: 08-04-2020
-- Description:	SP to Delete Serie records
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteSerieById] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Series]
           SET
		   [SerieDeleted] = 1
     WHERE [Id] = @Id
END