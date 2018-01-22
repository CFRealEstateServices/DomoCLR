CREATE TABLE [dbo].[DOMOUserDomainsToBlockFromManagementViaAPI] (
    [domain]      VARCHAR (200) NOT NULL,
    [emailDomain] VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_DOMOUserDomainsToBlockFromManagementViaAPI] PRIMARY KEY CLUSTERED ([domain] ASC, [emailDomain] ASC)
);

