CREATE TABLE [Lion].[Tags]
(
    [BundleId] INT NOT NULL IDENTITY (1,1) CONSTRAINT PK_Tags_BundleId PRIMARY KEY,
    [Bundle] NVARCHAR(128) NOT NULL,
    [Namespace] NVARCHAR(128) NOT NULL,
    [Comment] NVARCHAR(512) NULL,
    [CreatedAt] DATETIME2 (7) NOT NULL CONSTRAINT DF_Tags_CreatedAt DEFAULT (GETUTCDATE()),
    [ModifiedAt] DATETIME2 (7) NULL
)
GO
ALTER TABLE [Lion].[Tags]
    ADD CONSTRAINT AK_Tags_NamespaceBundle UNIQUE ([Namespace], [Bundle])