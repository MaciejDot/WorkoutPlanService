CREATE FUNCTION [Workout].[CheckExerciseIdConstraint]
(
	 @ExerciseId INT
)
RETURNS BIT
AS
BEGIN
    IF EXISTS (SELECT* FROM Workout.Exercise WHERE ExerciseId = @ExerciseId)
        return 1
    return 0
END
