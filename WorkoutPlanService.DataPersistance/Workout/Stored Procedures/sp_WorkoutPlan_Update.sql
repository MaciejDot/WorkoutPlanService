CREATE PROCEDURE [Workout].[sp_WorkoutPlan_Update]
	@WorkoutName NVARCHAR(400),
	@Username NVARCHAR(100),
	@WorkouPlanVersionId UNIQUEIDENTIFIER,
	@IsPublic BIT,
	@Created DATETIME2(7),
	@Description NVARCHAR(1000)
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

	INSERT INTO [Workout].[WorkoutPlanVersion] 
		([Id], [WorkoutPlanId], [Created], [IsPublic], [Description])
	VALUES 
		(@WorkouPlanVersionId, @WorkoutPlanId, @Created, @IsPublic, @Description);
RETURN 0