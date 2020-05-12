CREATE TABLE [dbo].[Type] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Term]          NVARCHAR (50) NOT NULL,
    [RecordCreated] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7) NOT NULL,
    [RecordDeleted] DATETIME2 (7) NULL,
    [TypeDeleted]   BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_TypeTerms] UNIQUE NONCLUSTERED 
	(
		[Term] ASC
	)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateType] ON [dbo].[Type]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Type]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Type].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteType] ON [dbo].[Type]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Type]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[Type] T on T.Id = i.Id
	WHERE i.TypeDeleted = 1
END

GO

CREATE INDEX [IX_Type_TypeDeleted] ON [dbo].[Type] ([TypeDeleted])
