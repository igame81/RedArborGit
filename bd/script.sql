
CREATE TABLE [dbo].[Employees] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [CompanyId] INT            NOT NULL,
    [CreatedOn] DATETIME       NULL,
    [DeletedOn] DATETIME       NULL,
    [Email]     NVARCHAR (100) NOT NULL,
    [Fax]       NVARCHAR (11)  NULL,
    [Name]      NVARCHAR (100) NULL,
    [Lastlogin] DATETIME       NULL,
    [Password]  NVARCHAR (100) NOT NULL,
    [PortalId]  INT            NOT NULL,
    [RoleId]    INT            NOT NULL,
    [StatusId]  INT            NOT NULL,
    [Telephone] NVARCHAR (11)  NULL,
    [UpdatedOn] DATETIME       NULL,
    [Username]  NVARCHAR (100) NOT NULL
);
