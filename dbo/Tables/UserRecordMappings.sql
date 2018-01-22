CREATE TABLE [dbo].[UserRecordMappings] (
    [domain]         VARCHAR (200) NOT NULL,
    [DOMOUserId]     INT           NOT NULL,
    [ExternalUserId] INT           NOT NULL,
    CONSTRAINT [PK_UserRecordMappings_1] PRIMARY KEY CLUSTERED ([domain] ASC, [DOMOUserId] ASC)
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_UserRecordMappings_Column] ON [dbo].[UserRecordMappings] ([domain], [ExternalUserId])

