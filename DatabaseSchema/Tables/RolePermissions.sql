CREATE TABLE [dbo].[RolePermissions]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [RoleId] INT NOT NULL, 
    [PermissionId] INT NOT NULL, 
    CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]),
    CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions]([Id])
)
