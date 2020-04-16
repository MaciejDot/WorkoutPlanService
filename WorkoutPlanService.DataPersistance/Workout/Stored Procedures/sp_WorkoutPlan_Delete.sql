CREATE PROCEDURE [Workout].[sp_WorkoutPlan_Delete]
	@WorkoutName NVARCHAR(400),
	@Username NVARCHAR(100),
	@DeactivationDate DATETIME2(7)

AS
	DECLARE @WorkoutPlanId UNIQUEIDENTIFIER=
	(
		SELECT 
			wp.[Id]
		FROM
			[Workout].[WorkoutPlan] wp
			LEFT JOIN [Security].[Users] u 
				ON u.[Id] = wp.[UserId]
		WHERE
			u.[Name] = @Username
			AND wp.[Name] = @WorkoutName
			AND wp.DeactivationDate IS NULL
	);

	UPDATE 
		[Workout].[WorkoutPlan] 
	SET 
		[DeactivationDate] = @DeactivationDate
	WHERE
		[Id] = @WorkoutPlanId;
RETURN 0