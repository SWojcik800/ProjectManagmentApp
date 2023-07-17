CREATE TABLE [dbo].[UserRoles]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserId] BIGINT NOT NULL, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
