CREATE TABLE [dbo].[SubSeries] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Title]           NVARCHAR (100) NOT NULL,
    [DateStart]       DATE           NULL,
    [DateEnd]         DATE           NULL,
    [SeriesId]        INT            NULL,
    [RecordCreated]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated]   DATETIME2 (7)  NOT NULL,
    [RecordDeleted]   DATETIME2 (7)  NULL,
    [SubSerieDeleted] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SubSeries] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateSubSeries] ON [dbo].[SubSeries]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[SubSeries]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[SubSeries].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteSubSerie] ON [dbo].[SubSeries]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[SubSeries]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[SubSeries] S on S.Id = i.Id
	WHERE i.SubSerieDeleted = 1
END

GO

CREATE INDEX [IX_SubSeries_SubSerieDeleted] ON [dbo].[SubSeries] ([SubSerieDeleted])
