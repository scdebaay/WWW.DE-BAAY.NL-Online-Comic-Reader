CREATE TABLE [dbo].[Authors] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]     NVARCHAR (50) NOT NULL,
    [MiddleName]    NVARCHAR (50) NULL,
    [LastName]      NVARCHAR (50) NULL,
    [DateBirth]     DATE          NULL,
    [DateDeceased]  DATE          NULL,
    [Active]        BIT           NULL,
    [RecordCreated] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7) NOT NULL,
    [RecordDeleted] DATETIME2 (7) NULL,
    [AuthorDeleted] BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Authors] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_AuthorNames] UNIQUE NONCLUSTERED 
	(
		[FirstName],[MiddleName],[LastName]
	)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateAuthors] ON [dbo].[Authors]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN
	UPDATE [dbo].[Authors]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Authors].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteAuthor] ON [dbo].[Authors]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Authors]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[Authors] A on A.Id = i.Id
	WHERE i.AuthorDeleted = 1
END

GO

CREATE INDEX [IX_Authors_AuthorDeleted] ON [dbo].[Authors] ([AuthorDeleted])
