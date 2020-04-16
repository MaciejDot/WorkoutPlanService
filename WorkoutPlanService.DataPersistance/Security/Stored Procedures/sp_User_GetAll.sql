CREATE PROCEDURE [Security].[sp_User_GetAll]
AS
	SELECT 
		[Name]
	FROM
		[Security].[Users]
RETURN 0