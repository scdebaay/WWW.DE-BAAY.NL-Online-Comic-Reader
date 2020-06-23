-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateComicByIdIngest]
	-- Add the parameters for the stored procedure here
	@Id int,
    @Name nvarchar(100), 
	@Path nvarchar(255),
	@TotalPages int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Comics]
           SET
           [Name] = @Name
           ,[Path] = @Path
           ,[TotalPages] = @TotalPages
     WHERE [Id] = @Id
END