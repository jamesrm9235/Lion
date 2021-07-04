CREATE TABLE [Lion].[Bundles]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[NamespaceId] INT NOT NULL,
	[Key] NVARCHAR (255) NOT NULL,
	CONSTRAINT PK_Bundles_Id PRIMARY KEY ([Id]),
	CONSTRAINT FK_Bundles_Namespaces FOREIGN KEY ([NamespaceId]) REFERENCES [Lion].[Namespaces] ([Id])
		ON DELETE CASCADE,
	CONSTRAINT AK_Bundles_NamespaceIdKey UNIQUE ([NamespaceId], [Key])
);