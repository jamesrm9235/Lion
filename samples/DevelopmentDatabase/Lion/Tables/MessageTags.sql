CREATE TABLE [Lion].[MessageTags]
(
    [MessageId] INT NOT NULL CONSTRAINT FK_MessageTags_Messages REFERENCES [Lion].[Messages] ([MessageId])
        ON DELETE CASCADE,
    [BundleId] INT NOT NULL CONSTRAINT FK_MessageTags_Tags REFERENCES [Lion].[Tags] ([BundleId]),
    CONSTRAINT PK_BundleId_MessageId PRIMARY KEY ([BundleId], [MessageId])
)