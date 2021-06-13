CREATE TABLE [Lion].[Messages]
(
    [MessageId] INT NOT NULL IDENTITY (1,1) CONSTRAINT PK_Messages_MessageId PRIMARY KEY,
    [Language] NVARCHAR (16) NOT NULL,
    [Value] NVARCHAR (MAX) NULL,
    [CreatedAt] DATETIME2 (7) NOT NULL CONSTRAINT DF_Messages_Created DEFAULT (GETUTCDATE()),
    [ModifiedAt] DATETIME2 (7) NULL
)