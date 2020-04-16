CREATE PROCEDURE [Workout].[sp_Exercise_GetAll]
AS
	SELECT 
		[Id], 
		[Name]
	FROM
		[Workout].[Exercise]
RETURN 0