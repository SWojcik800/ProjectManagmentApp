CREATE TABLE [dbo].[Users]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [CK_Users_UserName] CHECK (LEN(UserName) >= 4),
    CONSTRAINT [CK_Users_Password] CHECK (LEN(Password) >= 6), 
    CONSTRAINT [AK_Users_UserName] UNIQUE ([UserName])
)
