-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Author records
-- =============================================
CREATE PROCEDURE spUpdateAuthorById 
	-- Add the parameters for the stored procedure here
	@Id int,
    @FirstName nvarchar(50),
    @MiddleName nvarchar(50),
    @LastName nvarchar(50),
    @DateBirth date,
	@DateDeceased date,
	@Active bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Authors]
           SET
           [FirstName] = @FirstName
          ,[MiddleName] = @MiddleName
          ,[LastName] = @LastName
          ,[DateBirth] = @DateBirth
          ,[DateDeceased] = @DateDeceased
          ,[Active] = @Active
     WHERE [Id] = @Id
END