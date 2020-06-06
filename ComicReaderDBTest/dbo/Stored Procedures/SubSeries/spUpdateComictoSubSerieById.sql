-- =============================================
-- Author:		Solino de Baay
-- Create date: 09-05-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE spUpdateComicToSubSerieById 
	-- Add the parameters for the stored procedure here
	@Id int,    
    @SubSerieId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [SubSeriesId] = @SubSerieId
     WHERE [Id] = @Id
END