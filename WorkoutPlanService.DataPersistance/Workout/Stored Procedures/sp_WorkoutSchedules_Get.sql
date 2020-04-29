CREATE PROCEDURE [Workout].[sp_WorkoutSchedules_Get]
	@username nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		wsv.[ExternalId],
		wsv.[WorkoutPlanExternalId],
		wsv.[FirstDate],
		wsv.[Recurrence],
		wsv.[RecurringTimes]
	FROM
		[Workout].[WorkoutScheduleVersion] wsv
		LEFT JOIN [Workout].[WorkoutPlanVersion] wpv ON wsv.[WorkoutPlanExternalId] = wpv.[ExternalId]
		LEFT JOIN [Security].[Users] u ON wpv.[UserId] = u.[Id]
	WHERE 
		wsv.IsActive = 1
		AND wpv.IsActive = 1
		AND u.[Name] = @username
		AND wsv.Id = (
			SELECT TOP 1 [Id]
			FROM [Workout].[WorkoutScheduleVersion] wsv1
			WHERE wsv1.ExternalId = wsv.ExternalId
			ORDER BY wsv1.Created
			DESC
		)
		
END

