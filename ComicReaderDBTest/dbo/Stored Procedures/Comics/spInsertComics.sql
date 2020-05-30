-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to insert Comic records
-- =============================================
CREATE PROCEDURE spInsertComics 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(100), 
	@Path nvarchar(255),
	@TotalPages int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Comics]
           ([Name]
           ,[Path]
           ,[TotalPages]
           ,[RecordUpdated])
     VALUES
           (@Name
           ,@Path
           ,@TotalPages
           ,CONVERT (date, CURRENT_TIMESTAMP))
END