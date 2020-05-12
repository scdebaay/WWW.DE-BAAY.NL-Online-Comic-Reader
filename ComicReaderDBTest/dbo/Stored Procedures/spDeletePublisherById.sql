-- =============================================
-- Author:		Solino de Baay
-- Create date: 11-05-2020
-- Description:	SP to Delete Publisher records
-- =============================================
CREATE PROCEDURE [dbo].[spDeletePublisherById] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Publisher]
           SET
		   [PublisherDeleted] = 1
     WHERE [Id] = @Id
END