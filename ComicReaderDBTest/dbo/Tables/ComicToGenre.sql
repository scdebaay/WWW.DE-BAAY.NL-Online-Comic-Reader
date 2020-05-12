CREATE TABLE [dbo].[ComicToGenre] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ComicId]       INT           NOT NULL,
    [GenreId]       INT           NOT NULL,
    [RecordCreated] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7) NOT NULL,
    [RecordDeleted] DATETIME2 (7) NULL,
    [AssocDeleted]  BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ComicToGenre] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_ComicToGenreKeys] UNIQUE NONCLUSTERED
    (
        [ComicId], [GenreId]
    ),
    CONSTRAINT [FK_ComicToGenre_Comics] FOREIGN KEY ([ComicId]) REFERENCES [dbo].[Comics] ([Id]),
    CONSTRAINT [FK_ComicToGenre_Genres] FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genres] ([Id])
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateComicToGenre] ON [dbo].[ComicToGenre]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[ComicToGenre]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[ComicToGenre].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteGenreAssoc] ON [dbo].[ComicToGenre]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[ComicToGenre]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[ComicToGenre] C on C.Id = i.Id
	WHERE i.AssocDeleted = 1
END

GO

CREATE INDEX [IX_ComicToGenre_AssocDeleted] ON [dbo].[ComicToGenre] ([AssocDeleted])
