CREATE TABLE [dbo].[Publisher] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [RecordCreated]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated]    DATETIME2 (7)  NOT NULL,
    [RecordDeleted]    DATETIME2 (7)  NULL,
    [PublisherDeleted] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Publisher] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_PublisherNames] UNIQUE NONCLUSTERED 
	(
		[Name] ASC
	)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdatePublisher] ON [dbo].[Publisher]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Publisher]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Publisher].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeletePublisher] ON [dbo].[Publisher]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Publisher]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[Publisher] P on P.Id = i.Id
	WHERE i.PublisherDeleted = 1
END

GO

CREATE INDEX [IX_Publisher_PublisherDeleted] ON [dbo].[Publisher] ([PublisherDeleted])
