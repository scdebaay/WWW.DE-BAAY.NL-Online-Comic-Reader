﻿-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Type records
-- =============================================
CREATE PROCEDURE spUpdateTypeById 
	-- Add the parameters for the stored procedure here
	@Id int,
    @Term nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE [dbo].[Type]
           SET
           [Term] = @Term
     WHERE [Id] = @Id
END