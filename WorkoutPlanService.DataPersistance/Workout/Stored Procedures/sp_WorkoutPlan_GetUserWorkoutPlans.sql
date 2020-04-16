CREATE PROCEDURE [Workout].[sp_WorkoutPlan_GetUserWorkoutPlans]
	@username NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		wp.[Name],
		wpv.[Description],
		wp.[Created],
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
		[Workout].[WorkoutPlan] wp 
		JOIN [Security].[Users] u 
			ON wp.[UserId] = u.[Id]
			AND u.[Name] = @username
		LEFT JOIN [Workout].[WorkoutPlanVersion] wpv 
			ON wp.[Id] =wpv.[WorkoutPlanId]
		LEFT JOIN [Workout].[ExerciseExecutionPlan] eep
			ON eep.[WorkoutPlanVersionId] = wpv.[Id]
		LEFT JOIN [Workout].[Exercise] e
			ON e.[Id] = eep.[ExerciseId]
	WHERE
		wp.[DeactivationDate] IS NULL 
		AND wpv.[Id] 
			IN(
				SELECT TOP(1) 
					[Id] 
				FROM 
					[Workout].[WorkoutPlanVersion] wpv2
				WHERE
					wpv2.[WorkoutPlanId] = wp.[Id]
				ORDER BY
					CREATED DESC
			)
	ORDER BY
		u.[Id] ASC , 
		wp.[Created] ASC, 
		wpv.[Id] ASC , 
		eep.[Order] ASC
END