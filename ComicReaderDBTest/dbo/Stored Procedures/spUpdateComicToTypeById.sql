-- =============================================
-- Author:		Solino de Baay
-- Create date: 07-05-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE spUpdateComicToTypeById 
	-- Add the parameters for the stored procedure here
	@Id int,    
    @TypeId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [TypeId] = @TypeId
     WHERE [Id] = @Id
END