-- =============================================
-- Author:		Solino de Baay
-- Create date: 02-06-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE spUpdateComicToTypeByIds
	-- Add the parameters for the stored procedure here
	@Ids uComicIdTVP READONLY,    
    @TypeId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [TypeId] = @TypeId
	FROM [dbo].[Comics] INNER JOIN @Ids AS ids  
    ON [dbo].[Comics].[Id] = ids.Id
     WHERE [dbo].[Comics].[Id] = ids.Id
END