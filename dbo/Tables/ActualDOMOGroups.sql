CREATE TABLE [dbo].[ActualDOMOGroups] (
    [domain]      VARCHAR (200) NOT NULL,
    [creatorId]   INT           NOT NULL,
    [id]          INT           NOT NULL,
    [active]      BIT           NOT NULL,
    [isDefault]   BIT           NOT NULL,
    [memberCount] INT           NOT NULL,
    [name]        VARCHAR (200) NULL,
    CONSTRAINT [PK_ActualDOMOGroups] PRIMARY KEY NONCLUSTERED ([domain] ASC, [id] ASC)
);




GO
CREATE CLUSTERED INDEX [ClusteredIndex-20170316-153110]
    ON [dbo].[ActualDOMOGroups]([domain] ASC, [id] ASC);

