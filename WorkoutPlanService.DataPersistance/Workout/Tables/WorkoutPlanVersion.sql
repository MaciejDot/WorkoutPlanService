CREATE TABLE [Workout].[WorkoutPlanVersion] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [ExternalId]    UNIQUEIDENTIFIER NOT NULL,
    [IsActive]      BIT    NULL,
    [Name]          NVARCHAR (400)   NOT NULL,
    [Created]       DATETIME2 (7)    NOT NULL,
    [IsPublic]      BIT              NOT NULL,
    [Description]   NVARCHAR (1000)  NULL,
    CONSTRAINT [PK_WorkoutPlanVersion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkoutPlanVersion_User] FOREIGN KEY ([UserId]) REFERENCES [Security].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_WorkoutPlanVersion_UserId_IsActive]
    ON [Workout].[WorkoutPlanVersion]([UserId] ASC, [IsActive] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkoutPlanVersion_WorkoutPlanId_Created]
    ON [Workout].[WorkoutPlanVersion]([ExternalId] ASC, [Created] ASC);

