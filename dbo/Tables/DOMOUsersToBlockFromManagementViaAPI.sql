CREATE TABLE [dbo].[DOMOUsersToBlockFromManagementViaAPI] (
    [domain]     VARCHAR (200) NOT NULL,
    [email]      VARCHAR (200) NOT NULL,
    [externalID] INT           NOT NULL,
    CONSTRAINT [PK_DOMOUsersToBlockFromManagementViaAPI] PRIMARY KEY CLUSTERED ([domain] ASC, [externalID] ASC)
);

