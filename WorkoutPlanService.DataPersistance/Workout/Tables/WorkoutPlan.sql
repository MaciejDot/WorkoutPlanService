CREATE TABLE [Workout].[WorkoutPlan] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [UserId]           UNIQUEIDENTIFIER NOT NULL,
    [DeactivationDate] DATETIME2 (7)    NULL,
    [Name]             NVARCHAR (400)   NOT NULL,
    [Created]          DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_WorkoutPlan] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkoutPlans_User] FOREIGN KEY ([UserId]) REFERENCES [Security].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkoutPlan_UserId_DeactivationDate_Name]
    ON [Workout].[WorkoutPlan]([Name] ASC, [UserId] ASC, [DeactivationDate] ASC);

