﻿-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to insert Language records
-- =============================================
CREATE PROCEDURE spInsertLanguages
	-- Add the parameters for the stored procedure here
	@Term nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Language]
           (    [Term]
			  ,[RecordUpdated])
     VALUES
           (@Term
           ,CONVERT (date, CURRENT_TIMESTAMP))
		   RETURN IDENT_CURRENT('Language')
END