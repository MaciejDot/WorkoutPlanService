CREATE TABLE [Workout].[ExerciseExecutionPlan] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
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
    CONSTRAINT [FK_ExerciseExecutionPlan_Exercise_ExerciseId] FOREIGN KEY ([ExerciseId]) REFERENCES [Workout].[Exercise] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ExerciseExecutionPlan_WorkoutPlanVersions] FOREIGN KEY ([WorkoutPlanVersionId]) REFERENCES [Workout].[WorkoutPlanVersion] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ExerciseExecutionPlan_WorkoutPlanVersionId_Order_ExerciseId]
    ON [Workout].[ExerciseExecutionPlan]([WorkoutPlanVersionId] ASC, [Order] ASC, [ExerciseId] ASC);

