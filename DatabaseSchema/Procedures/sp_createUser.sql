CREATE PROCEDURE [dbo].[sp_createUser]
	@userName nvarchar(50),
	@password nvarchar(50)
AS
begin
INSERT INTO [dbo].[Users]
           ([UserName]
           ,[Password])
     VALUES
           (
           @userName, HASHBYTES('SHA2_512', @password)
           );

select SCOPE_IDENTITY();
end;