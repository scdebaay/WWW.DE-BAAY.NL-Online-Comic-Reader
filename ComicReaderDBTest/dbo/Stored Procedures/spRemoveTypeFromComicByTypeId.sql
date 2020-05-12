CREATE PROCEDURE [dbo].[spRemoveTypeFromComicByTypeId]
	@TypeId int	
AS
	BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [TypeId] = 1           
     WHERE [TypeId] = @TypeId
END