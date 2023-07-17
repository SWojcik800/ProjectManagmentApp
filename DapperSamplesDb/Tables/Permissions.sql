CREATE TABLE [dbo].[Permissions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Resource] NVARCHAR(50) NOT NULL, 
    [Create] BIT NOT NULL, 
    [Read] BIT NULL, 
    [Update] BIT NULL, 
    [Delete] BIT NULL, 
    [CustomPermission] NVARCHAR(50) NULL
)
