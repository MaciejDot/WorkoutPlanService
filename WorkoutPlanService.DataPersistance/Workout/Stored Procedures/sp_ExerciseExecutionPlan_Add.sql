CREATE PROCEDURE [Workout].[sp_ExerciseExecutionPlan_Add]
    @WorkoutPlanVersionId    UNIQUEIDENTIFIER,
    @Series                  INT             ,
    @MinReps                 INT             ,
    @MaxReps                 INT             ,
    @MinAdditionalKgs        INT             ,
    @MaxAdditionalKgs        INT             ,
    @ExerciseId              INT             ,
    @Order                   INT             ,
    @Description             NVARCHAR (1000) ,
    @Break                   INT             
AS
	INSERT INTO [Workout].[ExerciseExecutionPlan]
        ( [WorkoutPlanVersionId], [Series], [MinReps], [MaxReps], [MinAdditionalKgs], [MaxAdditionalKgs], [ExerciseId], [Order], [Description], [Break])
    VALUES
        ( @WorkoutPlanVersionId, @Series, @MinReps, @MaxReps, @MinAdditionalKgs, @MaxAdditionalKgs, @ExerciseId, @Order, @Description, @Break)
RETURN 0