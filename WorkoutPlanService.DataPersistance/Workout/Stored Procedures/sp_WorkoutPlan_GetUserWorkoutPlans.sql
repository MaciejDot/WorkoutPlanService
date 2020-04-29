CREATE PROCEDURE [Workout].[sp_WorkoutPlan_GetUserWorkoutPlans]
	@username NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		wpv.[ExternalId],
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
		[Workout].[WorkoutPlanVersion] wpv 
		JOIN [Security].[Users] u 
			ON wpv.[UserId] = u.[Id]
			AND u.[Name] = @username
		LEFT JOIN [Workout].[ExerciseExecutionPlan] eep
			ON eep.[WorkoutPlanVersionId] = wpv.[Id]
		LEFT JOIN [Workout].[Exercise] e
			ON e.[ExerciseId] = eep.[ExerciseId]
	WHERE
		wpv.IsActive = 1
		AND wpv.[Id] 
			IN(
				SELECT TOP(1) 
					[Id] 
				FROM 
					[Workout].[WorkoutPlanVersion] wpv2
				WHERE
					wpv2.[ExternalId] = wpv.[ExternalId]
				ORDER BY
					[Created] DESC
			)
	ORDER BY
		u.[Id] ASC , 
		wpv.[Id] ASC , 
		eep.[Order] ASC
END