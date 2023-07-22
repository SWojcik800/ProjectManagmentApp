CREATE TABLE [dbo].[TaskStatuses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(100) NULL
)
