CREATE TABLE [dbo].[Comics] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (100) NOT NULL,
    [Path]            NVARCHAR (255) NOT NULL,
    [TotalPages]      INT            NOT NULL,
    [PublicationDate] DATE           NULL,
    [ReleaseDate]     DATE           NULL,
    [LanguageId]      INT            NULL,
    [TypeId]          INT            NULL,
    [SeriesId]        INT            NULL,
    [SubSeriesId]     INT            NULL,
    [RecordCreated]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated]   DATETIME2 (7)  NOT NULL,
    [RecordDeleted]   DATETIME2 (7)  NULL,
    [ComicDeleted]    BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Comics] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Comics_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Comics_Series] FOREIGN KEY ([SeriesId]) REFERENCES [dbo].[Series] ([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Comics_SubSeries] FOREIGN KEY ([SubSeriesId]) REFERENCES [dbo].[SubSeries] ([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_Comics_Type] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[Type] ([Id]) ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [indexTitle]
    ON [dbo].[Comics]([Name] ASC);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateComics] ON [dbo].[Comics]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Comics]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Comics].[Id] IN ( SELECT Id FROM inserted )
END
GO

CREATE TRIGGER [dbo].[trgAfterDeleteComics] ON [dbo].[Comics]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Comics]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
    INNER JOIN [dbo].[Comics] C on C.Id = i.Id
	WHERE i.ComicDeleted = 1
END
GO

CREATE INDEX [IX_Comics_ComicDeleted] ON [dbo].[Comics] ([ComicDeleted])
