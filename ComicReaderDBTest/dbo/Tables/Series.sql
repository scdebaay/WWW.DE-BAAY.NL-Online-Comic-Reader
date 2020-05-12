CREATE TABLE [dbo].[Series] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Title]         NVARCHAR (100) NOT NULL,
    [DateStart]     DATE           NULL,
    [DateEnd]       DATE           NULL,
    [RecordCreated] DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7)  NOT NULL,
    [RecordDeleted] DATETIME2 (7)  NULL,
    [SerieDeleted]  BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Series] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateSeries] ON [dbo].[Series]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Series]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Series].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteSerie] ON [dbo].[Series]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Series]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[Series] S on S.Id = i.Id
	WHERE i.SerieDeleted = 1
END

GO

CREATE INDEX [IX_Series_SerieDeleted] ON [dbo].[Series] ([SerieDeleted])
