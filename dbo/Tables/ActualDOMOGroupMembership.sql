CREATE TABLE [dbo].[ActualDOMOGroupMembership] (
    [domain]  VARCHAR (200) NOT NULL,
    [groupId] INT           NOT NULL,
    [userId]  INT           NOT NULL,
    CONSTRAINT [PK_ActualDOMOGroupMembership] PRIMARY KEY CLUSTERED ([domain] ASC, [groupId] ASC, [userId] ASC)
);

