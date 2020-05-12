CREATE TABLE [dbo].[Language] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Term]            NVARCHAR (50) NOT NULL,
    [RecordCreated]   DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated]   DATETIME2 (7) NOT NULL,
    [RecordDeleted]   DATETIME2 (7) NULL,
    [LanguageDeleted] BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_LanguageTerms] UNIQUE NONCLUSTERED 
	(
		[Term] ASC
	)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateLanguage] ON [dbo].[Language]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Language]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Language].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteLanguage] ON [dbo].[Language]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Language]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[Language] L on L.Id = i.Id
	WHERE i.LanguageDeleted = 1
END

GO

CREATE INDEX [IX_Language_LanguageDeleted] ON [dbo].[Language] ([LanguageDeleted])
