CREATE PROCEDURE [Security].[sp_User_Get]
	@Name NVARCHAR(100)
AS
	SELECT 
		[Name]
	FROM
		[Security].[Users]
	WHERE
		[Name] = @Name
RETURN 0