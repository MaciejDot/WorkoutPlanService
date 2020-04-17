CREATE PROCEDURE [Security].[sp_User_GetAll]
AS
	SELECT DISTINCT
		[Name]
	FROM
		[Security].[Users]
RETURN 0