CREATE TABLE [dbo].[ComicToPublisher] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [ComicId]       INT           NOT NULL,
    [PublisherId]   INT           NOT NULL,
    [RecordCreated] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7) NOT NULL,
    [RecordDeleted] DATETIME2 (7) NULL,
    [AssocDeleted]  BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ComicToPublisher] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_ComicToPublisherKeys] UNIQUE NONCLUSTERED
    (
        [ComicId], [PublisherId]
    ),
    CONSTRAINT [FK_ComicToPublisher_Comics] FOREIGN KEY ([ComicId]) REFERENCES [dbo].[Comics] ([Id]) ON UPDATE CASCADE,
    CONSTRAINT [FK_ComicToPublisher_Publisher] FOREIGN KEY ([PublisherId]) REFERENCES [dbo].[Publisher] ([Id]) ON UPDATE CASCADE
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateComicToPublisher] ON [dbo].[ComicToPublisher]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[ComicToPublisher]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[ComicToPublisher].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeletePublisherAssoc] ON [dbo].[ComicToPublisher]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[ComicToPublisher]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[ComicToPublisher] C on C.Id = i.Id
	WHERE i.AssocDeleted = 1
END

GO

CREATE INDEX [IX_ComicToPublisher_AssocDeleted] ON [dbo].[ComicToPublisher] ([AssocDeleted])
