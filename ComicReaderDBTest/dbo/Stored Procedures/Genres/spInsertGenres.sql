-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to insert Genre records
-- =============================================
CREATE PROCEDURE spInsertGenres 
	-- Add the parameters for the stored procedure here
	@Term nvarchar(50)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Genres]
           (    [Term]
			  ,[RecordUpdated])
     VALUES
           (@Term
           ,CONVERT (date, CURRENT_TIMESTAMP))
END