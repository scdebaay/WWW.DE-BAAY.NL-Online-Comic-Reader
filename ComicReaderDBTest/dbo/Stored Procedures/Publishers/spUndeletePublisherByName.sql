CREATE PROCEDURE [dbo].[spUndeletePublisherByName]
	@Name NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Publisher]
           SET
           [RecordDeleted] = NULL,
		   [PublisherDeleted] = 0
     WHERE [Name] = @Name
	 RETURN IDENT_CURRENT('Publisher')
END