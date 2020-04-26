CREATE PROCEDURE [Workout].[sp_WorkoutPlan_Delete]

	@Username NVARCHAR(100),
	@WorkouPlanVersionId UNIQUEIDENTIFIER,
	@WorkoutName NVARCHAR(400),
	@ExternalId UNIQUEIDENTIFIER,
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
	INSERT INTO [Workout].[WorkoutPlanVersion]
		([Id], [Name], [IsActive],[UserId], [Created], [Description], [IsPublic], [ExternalId])
	VALUES
		(@WorkouPlanVersionId, @WorkoutName, 0, @UserId, @Created, NULL, 0, @ExternalId)

RETURN 0