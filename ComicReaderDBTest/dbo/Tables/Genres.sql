CREATE TABLE [dbo].[Genres] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Term]          NVARCHAR (50) NOT NULL,
    [RecordCreated] DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [RecordUpdated] DATETIME2 (7) NOT NULL,
    [RecordDeleted] DATETIME2 (7) NULL,
    [GenreDeleted]  BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [UQ_GenreTerms] UNIQUE NONCLUSTERED 
	(
		[Term] ASC
	)
);


GO

CREATE TRIGGER [dbo].[trgAfterUpdateGenres] ON [dbo].[Genres]
	AFTER INSERT, UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Genres]
	SET [RecordUpdated] = GETDATE()
	FROM inserted i
	WHERE [dbo].[Genres].[Id] IN ( SELECT Id FROM inserted )
END
GO


CREATE TRIGGER [dbo].[trgAfterDeleteGenre] ON [dbo].[Genres]
	AFTER UPDATE 
	AS
BEGIN
	IF TRIGGER_NESTLEVEL() > 1
	RETURN

	UPDATE [dbo].[Genres]
	SET [RecordDeleted] = GETDATE()
	FROM inserted i
	INNER JOIN [dbo].[Genres] G on G.Id = i.Id
	WHERE i.GenreDeleted = 1
END

GO

CREATE INDEX [IX_Genres_GenreDeleted] ON [dbo].[Genres] ([GenreDeleted])
