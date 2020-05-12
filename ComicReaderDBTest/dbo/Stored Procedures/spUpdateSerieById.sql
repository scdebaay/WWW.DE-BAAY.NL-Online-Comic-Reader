-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Serie records
-- =============================================
CREATE PROCEDURE spUpdateSerieById 
	-- Add the parameters for the stored procedure here
	@Id int,
    @Title nvarchar(100),
    @DateStart date,
    @DateEnd date
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Series]
           SET
           [Title] = @Title
           ,[DateStart] = @DateStart
           ,[DateEnd] = @DateEnd
     WHERE [Id] = @Id
END