-- =============================================
-- Author:		Solino de Baay
-- Create date: 08-05-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE spUpdateComicToSerieById 
	-- Add the parameters for the stored procedure here
	@Id int,    
    @SerieId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [SeriesId] = @SerieId
     WHERE [Id] = @Id
END