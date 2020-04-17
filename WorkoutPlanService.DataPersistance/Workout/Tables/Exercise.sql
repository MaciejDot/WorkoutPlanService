CREATE TABLE [Workout].[Exercise] (
    [Id] BIGINT IDENTITY NOT NULL,
    [ExerciseId] INT NOT NULL,
    [Name] NVARCHAR (400) NULL,
    [Created] DATETIME2(7) NOT NULL,
    [IsActive] BIT NOT NULL
    CONSTRAINT [PK_Exercise] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE NONCLUSTERED INDEX [IX_Exercise_ExerciseId] ON
[Workout].[Exercise]([ExerciseId] ASC);