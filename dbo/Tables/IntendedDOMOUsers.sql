CREATE TABLE [dbo].[IntendedDOMOUsers] (
    [domain]     VARCHAR (200) NOT NULL,
    [email]      VARCHAR (200) NOT NULL,
    [role]       VARCHAR (20)  NOT NULL,
    [name]       VARCHAR (200) NOT NULL,
    [externalID] INT           NOT NULL,
    CONSTRAINT [PK_IntendedDOMOUsers] PRIMARY KEY CLUSTERED ([domain] ASC, [externalID] ASC)
);

