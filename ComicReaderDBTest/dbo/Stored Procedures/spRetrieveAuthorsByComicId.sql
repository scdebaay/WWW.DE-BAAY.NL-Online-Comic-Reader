CREATE PROCEDURE [dbo].[spRetrieveAuthorsByComicId]
	@Id int	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
    [dbo].[Authors].[Id],
	[dbo].[Authors].[FirstName], 
	[dbo].[Authors].[MiddleName], 
	[dbo].[Authors].[LastName],
	[dbo].[Authors].[DateBirth],
	[dbo].[Authors].[DateDeceased],
	[dbo].[Authors].[Active]
FROM
    [dbo].[Authors]
INNER JOIN
    ([dbo].[Comics] INNER JOIN [dbo].[ComicToAuthors] 
	ON [dbo].[Comics].[Id] = [dbo].[ComicToAuthors].[ComicId]) 
	ON [dbo].[Authors].[Id] = [dbo].[ComicToAuthors].[AuthorId]
WHERE [dbo].[Comics].[Id] = @Id AND [ComicDeleted] = 0;
END