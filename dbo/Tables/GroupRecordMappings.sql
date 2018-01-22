CREATE TABLE [dbo].[GroupRecordMappings] (
    [domain]          VARCHAR (200) NOT NULL,
    [DOMOGroupId]     INT           NOT NULL,
    [externalGroupId] INT           NOT NULL,
    CONSTRAINT [PK_GroupRecordMappings_1] PRIMARY KEY CLUSTERED ([domain] ASC, [DOMOGroupId] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_GroupRecordMappings_Column] ON [dbo].[GroupRecordMappings] ([domain], [externalGroupId])
