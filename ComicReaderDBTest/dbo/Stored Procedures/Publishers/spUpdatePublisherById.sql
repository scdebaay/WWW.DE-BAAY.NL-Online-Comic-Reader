-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Publisher records
-- =============================================
CREATE PROCEDURE spUpdatePublisherById 
	-- Add the parameters for the stored procedure here
	@Id int,
    @Name nvarchar(100)    
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Publisher]
           SET
           [Name] = @Name
     WHERE [Id] = @Id
END