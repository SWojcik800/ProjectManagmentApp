CREATE TABLE [dbo].[UserProjects]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserId] BIGINT NOT NULL, 
    CONSTRAINT [FK_UserProjects_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    [ProjectId] BIGINT NOT NULL, 
    CONSTRAINT [FK_UserProjects_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) 
)
