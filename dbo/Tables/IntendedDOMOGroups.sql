CREATE TABLE [dbo].[IntendedDOMOGroups] (
    [domain]     VARCHAR (200) NOT NULL,
    [active]     BIT           NOT NULL,
    [isDefault]  BIT           NOT NULL,
    [name]       VARCHAR (200) NULL,
    [externalID] INT           NOT NULL,
    CONSTRAINT [PK_IntendedDOMOGroups] PRIMARY KEY NONCLUSTERED ([domain] ASC, [externalID] ASC)
);

