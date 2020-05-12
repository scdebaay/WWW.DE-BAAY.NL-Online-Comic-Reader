-- =============================================
-- Author:		Solino de Baay
-- Create date: 09-05-2020
-- Description:	SP to insert SubSerie records
-- =============================================
CREATE PROCEDURE spInsertSubSeries 
	-- Add the parameters for the stored procedure here
	@Title nvarchar(100),
	@DateStart date,
	@DateEnd date,
	@SerieId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[SubSeries]
           (    [Title]
				,[DateStart]
				,[DateEnd]
				,[SeriesId]
			  ,[RecordUpdated])
     VALUES
           (@Title
		   ,@DateStart
		   ,@DateEnd
		   ,@SerieId
           ,CONVERT (date, CURRENT_TIMESTAMP))
END