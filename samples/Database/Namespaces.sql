﻿CREATE TABLE [Lion].[Namespaces]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[Key] NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_Namespaces_Id PRIMARY KEY ([Id]),
	CONSTRAINT UX_Namespaces_Key UNIQUE ([Key])
);