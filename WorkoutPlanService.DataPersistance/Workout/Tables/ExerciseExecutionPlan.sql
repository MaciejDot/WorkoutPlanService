CREATE TABLE [Workout].[ExerciseExecutionPlan] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [WorkoutPlanVersionId] UNIQUEIDENTIFIER NOT NULL,
    [Series]               INT              NOT NULL,
    [MinReps]              INT              NOT NULL,
    [MaxReps]              INT              NOT NULL,
    [MinAdditionalKgs]     INT              NOT NULL,
    [MaxAdditionalKgs]     INT              NOT NULL,
    [ExerciseId]           INT              NOT NULL,
    [Order]                INT              NOT NULL,
    [Description]          NVARCHAR (1000)  NULL,
    [Break]                INT              NOT NULL,
    CONSTRAINT [PK_ExerciseExecutionPlan] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CHECK_ExerciseExecutionPlan_Exercise_ExerciseId] CHECK([Workout].[CheckExerciseIdConstraint](ExerciseId) = 1),
    CONSTRAINT [FK_ExerciseExecutionPlan_WorkoutPlanVersions] FOREIGN KEY ([WorkoutPlanVersionId]) REFERENCES [Workout].[WorkoutPlanVersion] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ExerciseExecutionPlan_WorkoutPlanVersionId_Order_ExerciseId]
    ON [Workout].[ExerciseExecutionPlan]([WorkoutPlanVersionId] ASC, [Order] ASC, [ExerciseId] ASC);

