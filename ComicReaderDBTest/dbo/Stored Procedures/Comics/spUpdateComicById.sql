-- =============================================
-- Author:		Solino de Baay
-- Create date: 13-04-2020
-- Description:	SP to update Comic records
-- =============================================
CREATE PROCEDURE spUpdateComicById 
	-- Add the parameters for the stored procedure here
	@Id int,
    @Name nvarchar(100), 
	@Path nvarchar(255),
	@TotalPages int,
	@PublicationDate date,
    @ReleaseDate date,
    @LanguageId int = 1,
    @TypeId int = 1,
    @SeriesId int = 1,
    @SubSeriesId int = 1
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
           ,[PublicationDate] = CAST(@PublicationDate AS DATE)
           ,[ReleaseDate] = CAST(@ReleaseDate AS DATE)
           ,[LanguageId] = @LanguageId
           ,[TypeId] = @TypeId
           ,[SeriesId] = @SeriesId
           ,[SubSeriesId] = @SubSeriesId
     WHERE [Id] = @Id
END