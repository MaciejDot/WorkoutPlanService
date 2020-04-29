CREATE PROCEDURE [Workout].[sp_WorkoutSchedule_Add]
        @ExternalId UNIQUEIDENTIFIER,
        @Recurrence INT,
        @FirstDate DATETIME2(7),
        @RecurringTimes INT,
        @WorkoutPlanExternalId UNIQUEIDENTIFIER,
        @Created DATETIME2(7),
        @IsActive BIT
AS
	INSERT INTO [Workout].[WorkoutScheduleVersion](ExternalId, Recurrence, FirstDate, RecurringTimes, WorkoutPlanExternalId, Created, IsActive)
    VALUES (@ExternalId,@Recurrence,@FirstDate,@RecurringTimes,@WorkoutPlanExternalId, @Created, @IsActive);
RETURN 0
