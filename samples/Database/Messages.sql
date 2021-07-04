CREATE TABLE [Lion].[Messages]
(
	[BundleId] INT NOT NULL,
	[Value] NVARCHAR (MAX) NULL,
	[Language] NVARCHAR(16) NOT NULL,
	[Comment] NVARCHAR (255) NULL,
	[CreatedAt] DATETIME2 (7) NOT NULL CONSTRAINT DF_Messages_CreatedAt DEFAULT (GETUTCDATE()),
	[ModifiedAt] DATETIME2 (7) NOT NULL CONSTRAINT DF_Messages_ModifiedAt DEFAULT (GETUTCDATE()),
	CONSTRAINT PK_Messages_BundleIdLanguage PRIMARY KEY ([BundleId], [Language]),
	CONSTRAINT FK_Messages_Bundles FOREIGN KEY ([BundleId]) REFERENCES [Lion].[Bundles] ([Id])
		ON DELETE CASCADE
);