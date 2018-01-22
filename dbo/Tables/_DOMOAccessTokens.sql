CREATE TABLE [dbo].[_DOMOAccessTokens] (
    [AccessToken] VARCHAR (2000) NOT NULL,
    [CreatedAt]   DATETIME       NOT NULL,
    [ExpiresAt]   DATETIME       NOT NULL,
    [DOMODomain]  VARCHAR (50)   NOT NULL,
    [Scope]       VARCHAR (10)   NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170316-145857]
    ON [dbo].[_DOMOAccessTokens]([DOMODomain] ASC, [Scope] ASC, [ExpiresAt] ASC);

