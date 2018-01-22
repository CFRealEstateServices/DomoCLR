CREATE TABLE [dbo].[IntendedDOMOGroupMembership] (
    [domain]          VARCHAR (200) NOT NULL,
    [externalGroupId] INT           NOT NULL,
    [externalUserId]  INT           NOT NULL,
    CONSTRAINT [PK_IntendedDOMOGroupMembership] PRIMARY KEY CLUSTERED ([domain] ASC, [externalGroupId] ASC, [externalUserId] ASC)
);

