CREATE PROCEDURE [Workout].[sp_WorkoutPlan_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		u.[Name] [Username],
		wpv.[Id],
		wpv.[Name],
		wpv.[Description],
		wpv.[Created],
		wpv.[IsPublic],
		eep.[Id],
		eep.[Series],
		eep.[Break],
		eep.[MaxAdditionalKgs],
		eep.[MinAdditionalKgs],
		eep.[ExerciseId],
		e.[Name] [ExerciseName],
		eep.[Order],
		eep.[Description],
		eep.[MinReps],
		eep.[MaxReps]
	FROM 
		[Security].[Users] u 
		LEFT JOIN [Workout].[WorkoutPlanVersion] wpv 
			ON wpv.[UserId] = u.[Id]
		LEFT JOIN [Workout].[ExerciseExecutionPlan] eep
			ON eep.[WorkoutPlanVersionId] = wpv.[Id]
		LEFT JOIN [Workout].[Exercise] e
			ON e.[Id] = eep.[ExerciseId]
	WHERE
		wpv.IsActive = 1
		AND wpv.[Id] 
			IN(
				SELECT TOP(1) 
					[Id] 
				FROM 
					[Workout].[WorkoutPlanVersion] wpv2
				WHERE
					wpv2.[Name] = wpv.[Name]
				ORDER BY
					CREATED DESC
			)
		OR wpv.[Id] IS NULL
	ORDER BY
		u.[Id] ASC ,
		wpv.[Id] ASC , 
		eep.[Order] ASC
END