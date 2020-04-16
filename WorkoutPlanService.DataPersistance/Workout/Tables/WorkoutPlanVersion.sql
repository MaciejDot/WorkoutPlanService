CREATE TABLE [Workout].[WorkoutPlanVersion] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [WorkoutPlanId] UNIQUEIDENTIFIER NOT NULL,
    [Created]       DATETIME2 (7)    NOT NULL,
    [IsPublic]      BIT              NOT NULL,
    [Description]   NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_WorkoutPlanVersion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkoutPlanVersions_WorkoutPlan] FOREIGN KEY ([WorkoutPlanId]) REFERENCES [Workout].[WorkoutPlan] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkoutPlanVersion_WorkoutPlanId_Created]
    ON [Workout].[WorkoutPlanVersion]([WorkoutPlanId] ASC, [Created] ASC);

