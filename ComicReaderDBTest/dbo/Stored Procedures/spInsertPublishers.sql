-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to insert Publisher records
-- =============================================
CREATE PROCEDURE spInsertPublishers
	-- Add the parameters for the stored procedure here
	@Name nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Publisher]
           (    [Name]              
			  ,[RecordUpdated])
     VALUES
           (@Name
           ,CONVERT (date, CURRENT_TIMESTAMP))
END