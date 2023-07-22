CREATE TABLE [dbo].[Tasks]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(512) NULL, 
    [AssignedToUserId] BIGINT NULL, 
    CONSTRAINT [FK_Tasks_Users] FOREIGN KEY ([AssignedToUserId]) REFERENCES [Users]([Id]), 
    [CreationTime] DATETIME NOT NULL, 
    [TaskPriorityId] INT NOT NULL, 
    CONSTRAINT [FK_Tasks_TaskPriorities] FOREIGN KEY ([TaskPriorityId]) REFERENCES [TaskPriorities]([Id]),
    [AssignedToProjectId] BIGINT NULL, 
    [TaskStatusId] INT NULL, 
    CONSTRAINT [FK_Tasks_Projects] FOREIGN KEY ([AssignedToProjectId]) REFERENCES [Projects]([Id]), 
    CONSTRAINT [FK_Tasks_TaskStatuses] FOREIGN KEY ([TaskStatusId]) REFERENCES [TaskStatuses]([Id])

)
