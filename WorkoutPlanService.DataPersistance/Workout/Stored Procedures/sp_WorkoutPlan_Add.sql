CREATE PROCEDURE [Workout].[sp_WorkoutPlan_Add]

	@Username NVARCHAR(100),
	@WorkoutPlanId UNIQUEIDENTIFIER,
	@WorkouPlanVersionId UNIQUEIDENTIFIER,
	@WorkoutName NVARCHAR(400),
	@Description NVARCHAR(1000),
	@IsPublic BIT,
	@Created DATETIME2(7)

AS
	DECLARE @UserId UNIQUEIDENTIFIER =
	(
		SELECT TOP 1
			[Id]
		FROM
			[Security].[Users]
		WHERE
			[Name] = @Username
	)
	INSERT INTO [Workout].[WorkoutPlan] 
		([Id], [UserId], [DeactivationDate], [Name], [Created])
	VALUES
		(@WorkoutPlanId, @UserId, NULL, @WorkoutName, @Created)

	INSERT INTO [Workout].[WorkoutPlanVersion]
		([Id], [WorkoutPlanId], [Created], [Description], [IsPublic])
	VALUES
		(@WorkouPlanVersionId, @WorkoutPlanId, @Created, @Description, @IsPublic)

RETURN 0