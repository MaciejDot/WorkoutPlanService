CREATE PROCEDURE [Workout].[sp_Exercise_GetAll]
AS
	SELECT 
		[ExerciseId] [Id], 
		[Name]
	FROM
		[Workout].[Exercise] e
	WHERE
		e.Id = (
			SELECT TOP 1
				[Id]
			FROM
				[Workout].[Exercise] e2
			WHERE
				e2.ExerciseId = e.ExerciseId
			ORDER BY
				e2.Created DESC
		)
		AND e.IsActive = 1
RETURN 0