CREATE PROCEDURE [Workout].[sp_ExerciseExecutionPlan_Add]
	@ExerciseExecutionPlanId UNIQUEIDENTIFIER,
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
        ([Id], [WorkoutPlanVersionId], [Series], [MinReps], [MaxReps], [MinAdditionalKgs], [MaxAdditionalKgs], [ExerciseId], [Order], [Description], [Break])
    VALUES
        (@ExerciseExecutionPlanId, @WorkoutPlanVersionId, @Series, @MinReps, @MaxReps, @MinAdditionalKgs, @MaxAdditionalKgs, @ExerciseId, @Order, @Description, @Break)
RETURN 0