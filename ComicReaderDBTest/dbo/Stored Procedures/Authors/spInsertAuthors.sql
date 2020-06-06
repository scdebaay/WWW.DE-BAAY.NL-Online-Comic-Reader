-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to insert Author records
-- =============================================
CREATE PROCEDURE spInsertAuthors 
	-- Add the parameters for the stored procedure here
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

	INSERT INTO [dbo].[Authors]
           (    [FirstName]
              ,[MiddleName]
              ,[LastName]
			  ,[DateBirth]
			  ,[DateDeceased]
			  ,[Active]
			  ,[RecordUpdated])
     VALUES
           (@FirstName
           ,@MiddleName
           ,@LastName
		   ,@DateBirth
		   ,@DateDeceased
		   ,@Active
           ,CONVERT (date, CURRENT_TIMESTAMP))
END