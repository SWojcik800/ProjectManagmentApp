CREATE FUNCTION [dbo].[fn_authorize]
(
	@userName nvarchar(50),
	@password nvarchar(50)
)
RETURNS BIT
AS
BEGIN
	declare @userPassword nvarchar(50);

	select @userPassword = Password from Users where UserName = @userName;

	IF HASHBYTES('SHA2_512', @password) = @userPassword
		RETURN 1;

	RETURN 0;
END;
