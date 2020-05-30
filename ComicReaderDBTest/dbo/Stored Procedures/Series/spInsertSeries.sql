-- =============================================
-- Author:		Solino de Baay
-- Create date: 08-05-2020
-- Description:	SP to insert Serie records
-- =============================================
CREATE PROCEDURE spInsertSeries 
	-- Add the parameters for the stored procedure here
	@Title nvarchar(100),
	@DateStart date,
	@DateEnd date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Series]
           (    [Title]
				,[DateStart]
				,[DateEnd]
			  ,[RecordUpdated])
     VALUES
           (@Title
		   ,@DateStart
		   ,@DateEnd
           ,CONVERT (date, CURRENT_TIMESTAMP))
		   RETURN IDENT_CURRENT('Series')
END