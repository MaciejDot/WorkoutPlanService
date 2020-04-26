CREATE PROCEDURE [Workout].[sp_WorkoutPlan_Add]

	@Username NVARCHAR(100),
	@WorkouPlanVersionId UNIQUEIDENTIFIER,
	@ExternalId UNIQUEIDENTIFIER,
	@WorkoutName NVARCHAR(400),
	@Description NVARCHAR(1000),
	@IsPublic BIT,
	@Created DATETIME2(7),
	@IsActive BIT

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
	INSERT INTO [Workout].[WorkoutPlanVersion]
		([Id], [Name], [IsActive], [UserId], [Created], [Description], [IsPublic], [ExternalId])
	VALUES
		(@WorkouPlanVersionId, @WorkoutName, @IsActive, @UserId, @Created, @Description, @IsPublic, @ExternalId)

RETURN 0