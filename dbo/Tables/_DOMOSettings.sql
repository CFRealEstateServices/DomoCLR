CREATE TABLE [dbo].[_DOMOSettings] (
    [ClientID]     VARCHAR (100) NOT NULL,
    [ClientSecret] VARCHAR (100) NOT NULL,
    [Scope]        VARCHAR (10)  NOT NULL,
    [DOMODomain]   VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK__DOMOSettings_1] PRIMARY KEY CLUSTERED ([Scope] ASC, [DOMODomain] ASC)
);

