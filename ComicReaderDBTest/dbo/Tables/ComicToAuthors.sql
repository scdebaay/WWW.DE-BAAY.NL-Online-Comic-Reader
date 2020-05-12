CREATE TABLE [dbo].[ComicToAuthors] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ComicId]       INT           NOT NULL,
    [AuthorId]      INT           NOT NULL,
    [RecordCreated] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7) NOT NULL,
    [RecordDeleted] DATETIME2 (7) NULL,
    [AssocDeleted]  BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ComicToAuthors] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_ComicToAuthorKeys] UNIQUE NONCLUSTERED
    (
        [ComicId], [AuthorId]
    ),
    CONSTRAINT [FK_ComicToAuthors_Authors] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Authors] ([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_ComicToAuthors_Comics] FOREIGN KEY ([ComicId]) REFERENCES [dbo].[Comics] ([Id])
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateComicToAuthors] ON [dbo].[ComicToAuthors]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN
	UPDATE [dbo].[ComicToAuthors]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[ComicToAuthors].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteAuthorAssoc] ON [dbo].[ComicToAuthors]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[ComicToAuthors]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[ComicToAuthors] C on C.Id = i.Id
	WHERE i.AssocDeleted = 1
END

GO

CREATE INDEX [IX_ComicToAuthors_AssocDeleted] ON [dbo].[ComicToAuthors] ([AssocDeleted])
