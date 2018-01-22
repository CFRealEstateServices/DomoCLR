CREATE TABLE [dbo].[ActualDOMOUsers] (
    [domain]    VARCHAR (200) NOT NULL,
    [id]        INT           NOT NULL,
    [title]     VARCHAR (200) NULL,
    [email]     VARCHAR (200) NOT NULL,
    [role]      VARCHAR (20)  NOT NULL,
    [name]      VARCHAR (200) NOT NULL,
    [createdAt] DATETIME      NOT NULL,
    [UpdatedAt] DATETIME      NOT NULL,
    [image]     VARCHAR (300) NOT NULL,
    CONSTRAINT [PK_ActualDOMOUsers] PRIMARY KEY NONCLUSTERED ([domain] ASC, [id] ASC)
);




GO
CREATE CLUSTERED INDEX [ClusteredIndex-20170316-153126]
    ON [dbo].[ActualDOMOUsers]([domain] ASC, [id] ASC);

