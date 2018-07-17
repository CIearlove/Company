CREATE TABLE [dbo].[Staff] (
    [Id]   INT           IDENTITY(1,1) NOT NULL ,
    [Name] NVARCHAR (100) NOT NULL,
    [Age]  INT           NOT NULL,
    [Sex]  CHAR (10)      NOT NULL,
    CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED ([Id] ASC)
);

