CREATE FUNCTION [Workout].[CheckWorkoutExternalIdConstraint]
(
	@ExternalId UNIQUEIDENTIFIER
)
RETURNS BIT
AS
BEGIN
    
    IF EXISTS (SELECT * 
        FROM [Workout].[WorkoutPlanVersion] wpv 
        WHERE ExternalId = @ExternalId )
        return 1
    return 0
END

